﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Photo
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public int AlbumID { get; set; }
		[Required]
		public string Location { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public int Rating { get; set; }
	}
}
