using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebMediaClient.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
		[Display(ResourceType = typeof(Resources), Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

		[Display(ResourceType = typeof(Resources), Name = "RememberBrowser")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
		[Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
		[Display(ResourceType = typeof(Resources), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
		[Required]
        [EmailAddress]
		[Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }

		[Required]
		[RegularExpression("^[a-zA-Z0-9_-]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Username")]
		public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UnmatchingPasssword")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
		[Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resources), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Resources), Name = "ConfirmPassword")]
		[Compare("Password", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "UnmatchingPasssword")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
		[Display(ResourceType = typeof(Resources), Name = "Email")]
        public string Email { get; set; }
    }
}
