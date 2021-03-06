﻿using Models;
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
		public int UserID { get; set; }
		[Required]
		//[RegularExpression("^[a-zA-Z0-9_-]*$")]
		public string Username { get; set; }
		//[RegularExpression("^[a-zA-Z]*$")]
		public string Name { get; set; }
		[Range(14, 100)]
		public int? Age { get; set; }
		public string Gender { get; set; }
		public int? PictureID { get; set; }
	}
}