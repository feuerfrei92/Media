﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class SectionModel
	{
		public int ID { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9]*$")]
		public string Name { get; set; }
		public int? ParentID { get; set; }
	}
}
