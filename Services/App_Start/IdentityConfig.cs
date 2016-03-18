using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Services.Models;
using Data;
using Models;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web.Mail;

namespace Services
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
		public class MyEmailService : IIdentityMessageService
		{
			public Task SendAsync(IdentityMessage message)
			{
				// Plug in your email service here to send an email.
				//return Task.FromResult(0);
				try
				{
					//var email = new MailMessage(new MailAddress("kolega_kz@abv.bg"),
					//							new MailAddress(message.Destination))
					//							{
					//								Subject = message.Subject,
					//								Body = message.Body,
					//								IsBodyHtml = true
					//							};


					//using (var client = new SmtpMail("smtp.abv.bg"))
					//{
					//	client.EnableSsl = true;
					//	client.Port = 465;
					//	client.UseDefaultCredentials = false;
					//	client.Credentials = new System.Net.NetworkCredential("kolega_kz", "17168934713");
					//	client.DeliveryMethod = SmtpDeliveryMethod.Network;

					//	await client.SendMailAsync(email);
					//}

					var email = new MailMessage();
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", "smtp.abv.bg");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "kolega_kz@abv.bg");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "17168934713");
					email.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");

					email.From = "kolega_kz@abv.bg";
					email.To = message.Destination;
					email.Subject = message.Subject;
					email.Body = message.Body;
					email.BodyFormat = MailFormat.Html;

					SmtpMail.SmtpServer = "smtp.abv.bg";
					SmtpMail.Send(email);

					return Task.FromResult(0);
				}
				catch
				{
					throw;
				}
			}
		}

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
			this.EmailService = new MyEmailService();
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<DBContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

		// Configure the application sign-in manager which is used in this application.
		public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
		{
			public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
				: base(userManager, authenticationManager)
			{
			}

			public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
			{
				return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie);
			}

			public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
			{
				return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
			}
		}
    }
}
