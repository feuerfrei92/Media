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
		public int OwnerID { get; set; }
		[Required]
		public byte[] Content { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
	}
}