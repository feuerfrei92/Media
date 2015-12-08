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
		[Required]
		public string Username { get; set; }
		public string Name { get; set; }
		public int? Age { get; set; }
		public Gender Gender { get; set; }
	}

	public class ProfileCriteriaViewModel
	{
		public string Name { get; set; }
		public int? MinimumAge { get; set; }
		public int? MaximumAge { get; set; }
		public GenderCriterion Gender { get; set; }
	}
}
