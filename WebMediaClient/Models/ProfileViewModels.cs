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
		public string Username { get; set; }
		[RegularExpression("^[a-zA-Z]*$")]
		public string Name { get; set; }
		[Range(14, 100)]
		public int? Age { get; set; }
		public Gender Gender { get; set; }
		public int? PictureID { get; set; }
	}

	public class ProfileCriteriaViewModel
	{
		[RegularExpression("^[a-zA-Z]*$")]
		public string Name { get; set; }
		[Range(14, 100)]
		public int? MinimumAge { get; set; }
		[Range(14, 100)]
		public int? MaximumAge { get; set; }
		public GenderCriterion Gender { get; set; }
	}
}
