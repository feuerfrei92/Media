﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class AlbumModel
	{
		public int ID { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9]*$")]
		public string Name { get; set; }
		public int OwnerID { get; set; }
		public bool IsProfile { get; set; }
		public bool IsInterest { get; set; }
		public int Size { get; set; }
		public int Rating { get; set; }
	}
}
