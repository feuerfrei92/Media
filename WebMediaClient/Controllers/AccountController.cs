using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebMediaClient.Models;
using Models;
using System.Security.Principal;
using System.Threading;
using System.Web.UI;
using System.Net.Http;
using System.Collections.Generic;

namespace WebMediaClient.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				string url = "http://localhost:8080/Token";
				var token = await HttpClientBuilder<LoginViewModel>.LoginAsync<TokenInfo>(model, url);
				token.Issued = DateTime.Now;
				token.Expired = token.Issued.AddMinutes(30);
				TempData["token"] = token.Access_Token;

				return RedirectToAction("SuccessfulLogin", "Home");
			}
			catch
			{
				ModelState.AddModelError("", "Invalid login attempt.");
				return View(model);
			}

			//// This doesn't count login failures towards account lockout
			//// To enable password failures to trigger account lockout, change to shouldLockout: true
			//var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
			//switch (result)
			//{
			//	case SignInStatus.Success:
			//		return RedirectToLocal(returnUrl);
			//	case SignInStatus.LockedOut:
			//		return View("Lockout");
			//	case SignInStatus.RequiresVerification:
			//		return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
			//	case SignInStatus.Failure:
			//	default:
			//		ModelState.AddModelError("", "Invalid login attempt.");
			//		return View(model);
			//}
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
			//if (!await SignInManager.HasBeenVerifiedAsync())
			//{
			//	return View("Error");
			//}
			try
			{
				string url = string.Format("http://localhost:8080/api/Account/VerifyCode?Provider={0}&ReturnUrl={1}&RememberMe={2}", provider, returnUrl, rememberMe);
				var verifyCodeModel = await HttpClientBuilder<Services.Models.CodeModels.VerifyCodeModel>.GetAsync(url, null);
				var viewModel = new VerifyCodeViewModel
				{
					Provider = verifyCodeModel.Provider,
					ReturnUrl = verifyCodeModel.ReturnUrl,
					RememberMe = verifyCodeModel.RememberMe,
				};
				return View(viewModel);
			}
			catch
			{
				return View("Error");
			}
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
			//if (!ModelState.IsValid)
			//{
			//	return View(model);
			//}

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
			//var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
			//switch (result)
			//{
			//	case SignInStatus.Success:
			//		return RedirectToLocal(model.ReturnUrl);
			//	case SignInStatus.LockedOut:
			//		return View("Lockout");
			//	case SignInStatus.Failure:
			//	default:
			//		ModelState.AddModelError("", "Invalid code.");
			//		return View(model);
			//}
			try
			{
				string url = "http://localhost:8080/api/Account/VerifyCode";
				var verifyCodeModel = new Services.Models.CodeModels.VerifyCodeModel
				{
					Code = model.Code,
					Provider = model.Provider,
					RememberBrowser = model.RememberBrowser,
					RememberMe = model.RememberMe,
					ReturnUrl = model.ReturnUrl,
				};
				var response = await HttpClientBuilder<Services.Models.CodeModels.VerifyCodeModel>.PostAsync<HttpResponseMessage>(verifyCodeModel, url, null);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return View("Error");
			}
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
				string url = "http://localhost:8080/api/Account/Register";
				var registeredModel = await HttpClientBuilder<RegisterViewModel>.PostAsync(model, url, null);
				//var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				//var result = await UserManager.CreateAsync(user, model.Password);
				//if (result.Succeeded)
				//{
				//	await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
				//	// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
				//	// Send an email with this link
				//	// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
				//	// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
				//	// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

				//	return RedirectToAction("Index", "Home");
				//}
				//AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
			return RedirectToAction("ConfirmRegistration");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
			string url = string.Format("http://localhost:8080/api/Account/ConfirmEmail?UserId={0}&Code={1}", userId, code);
			var response = await HttpClientBuilder<HttpResponseMessage>.GetEmptyAsync(url, null);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
				return View("ConfirmEmail");
			else return View("Error");
			//if (userId == null || code == null)
			//{
			//	return View("Error");
			//}
			//var result = await UserManager.ConfirmEmailAsync(userId, code);
			//return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
			try
			{
				string url = string.Format("http://localhost:8080/api/Account/SendCode?ReturnUrl={0}&RememberMe={1}", returnUrl, rememberMe);
				var sendCodeModel = await HttpClientBuilder<Services.Models.CodeModels.SendCodeModel>.GetAsync(url, null);
				var viewModel = new SendCodeViewModel
				{
					Providers = sendCodeModel.Providers,
					ReturnUrl = sendCodeModel.ReturnUrl,
					RememberMe = sendCodeModel.RememberMe,
				};
				//var userId = await SignInManager.GetVerifiedUserIdAsync();
				//if (userId == null)
				//{
				//	return View("Error");
				//}
				//var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
				//var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
				return View(viewModel);
			}
			catch
			{
				return View("Error");
			}
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
			if (ModelState.IsValid)
			{
				string url = "http://localhost:8080/api/Account/SendCode";
				var sendCodeModel = new Services.Models.CodeModels.SendCodeModel
				{
					Providers = model.Providers,
					RememberMe = model.RememberMe,
					ReturnUrl = model.ReturnUrl,
					SelectedProvider = model.SelectedProvider,
				};
				var registeredModel = await HttpClientBuilder<Services.Models.CodeModels.SendCodeModel>.PostAsync(sendCodeModel, url, null);
				var viewModel = new SendCodeViewModel
				{
					Providers = registeredModel.Providers,
					RememberMe = registeredModel.RememberMe,
					ReturnUrl = registeredModel.ReturnUrl,
				};
				return RedirectToAction("VerifyCode", new { Provider = viewModel.SelectedProvider, ReturnUrl = viewModel.ReturnUrl, RememberMe = viewModel.RememberMe });
			}

			// Generate the token and send it
			//if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			//{
			//	return View("Error");
			//}
			else
				return View("Error");
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

		[AllowAnonymous]
		public ActionResult ConfirmRegistration()
		{
			return View();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}