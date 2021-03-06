﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class TopicViewModel
	{
		public int ID { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Name")]
		public string Name { get; set; }
		public int SectionID { get; set; }
		public int AuthorID { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public string TopicType { get; set; }
	}

	public class TopicCriteriaViewModel
	{
		[RegularExpression("^[a-zA-Z0-9]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Name")]
		public string Name { get; set; }
		public int? SectionID { get; set; }
		public int? AuthorID { get; set; }
		public DateTime? DateCreatedFrom { get; set; }
		public DateTime? DateCreatedTo { get; set; }
		public DateTime? DateModifiedFrom { get; set; }
		public DateTime? DateModifiedTo { get; set; }
	}
}
