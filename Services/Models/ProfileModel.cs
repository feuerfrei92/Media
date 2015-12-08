using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class ProfileModel
	{
		public int ID { get; set; }
		[Required]
		public string Username { get; set; }
		public string Name { get; set; }
		public int? Age { get; set; }
		public Gender Gender { get; set; }
	}
}