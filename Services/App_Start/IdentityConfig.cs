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

namespace Services
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
		public class EmailService : IIdentityMessageService
		{
			public Task SendAsync(IdentityMessage message)
			{
				// Plug in your email service here to send an email.
				return Task.FromResult(0);
			}
		}

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
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
