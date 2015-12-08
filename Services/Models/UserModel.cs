using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class UserModel
	{
		public int ID { get; set; }
		[Required]
		public string Username { get; set; }
	}
}