using Models;
using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class ProfileViewModel
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9_-]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Username")]
		public string Username { get; set; }
		[RegularExpression("^[a-zA-Z]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Name")]
		public string Name { get; set; }
		[Range(14, 100)]
		[Display(ResourceType = typeof(Resources), Name = "Age")]
		public int? Age { get; set; }
		[Display(ResourceType = typeof(Resources), Name = "Gender")]
		public string Gender { get; set; }
		public int? PictureID { get; set; }
	}

	public class ProfileCriteriaViewModel
	{
		[RegularExpression("^[a-zA-Z]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Name")]
		public string Name { get; set; }
		[Range(14, 100)]
		[Display(ResourceType = typeof(Resources), Name = "MinimumAge")]
		public int? MinimumAge { get; set; }
		[Range(14, 100)]
		[Display(ResourceType = typeof(Resources), Name = "MaximumAge")]
		public int? MaximumAge { get; set; }
		[Display(ResourceType = typeof(Resources), Name = "Gender")]
		public string Gender { get; set; }
	}
}
