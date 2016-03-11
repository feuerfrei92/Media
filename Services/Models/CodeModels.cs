using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class CodeModels
	{
		public class SendCodeModel
		{
			public string SelectedProvider { get; set; }
			public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
			public string ReturnUrl { get; set; }
			public bool RememberMe { get; set; }
		}

		public class VerifyCodeModel
		{
			[Required]
			public string Provider { get; set; }

			[Required]
			[Display(Name = "Code")]
			public string Code { get; set; }
			public string ReturnUrl { get; set; }

			[Display(Name = "Remember this browser?")]
			public bool RememberBrowser { get; set; }

			public bool RememberMe { get; set; }
		}
	}
}