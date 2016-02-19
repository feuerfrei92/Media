﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class PhotoInViewModel
	{
		public int ID { get; set; }
		[Required]
		public int AlbumID { get; set; }
		public Image Content { get; set; }
		public DateTime DateCreated { get; set; }
		public int Rating { get; set; }
	}

	public class PhotoOutViewModel
	{
		public int ID { get; set; }
		[Required]
		public int AlbumID { get; set; }
		public byte[] Content { get; set; }
		public DateTime DateCreated { get; set; }
		public int Rating { get; set; }
	}
}
