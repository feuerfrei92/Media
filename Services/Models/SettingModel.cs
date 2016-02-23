using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class SettingModel
	{
		public int ID { get; set; }
		public int OwnerID { get; set; }
		[Required]
		public string OwnerType { get; set; }
		[Required]
		public string Publicity { get; set; }
	}
}