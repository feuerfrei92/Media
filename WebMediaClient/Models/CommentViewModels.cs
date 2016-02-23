using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class CommentViewModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		[Required]
		public string Text { get; set; }
		public int? ParentID { get; set; }
		public int TopicID { get; set; }
		public int AuthorID { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public int Rating { get; set; }
	}

	public class CommentCriteriaViewModel
	{
		public string Name { get; set; }
		public int? TopicID { get; set; }
		public int? AuthorID { get; set; }
		public DateTime? DateCreatedFrom { get; set; }
		public DateTime? DateCreatedTo { get; set; }
		public DateTime? DateModifiedFrom { get; set; }
		public DateTime? DateModifiedTo { get; set; }
	}
}
