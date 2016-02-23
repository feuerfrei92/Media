using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Comment
	{
		[Required]
		public int ID { get; set; }
		public string Name { get; set; }
		[Required]
		public string Text { get; set; }
		[Required]
		public int? ParentID { get; set; }
		[Required]
		public int TopicID { get; set; }
		[Required]
		public int AuthorID { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public int Rating { get; set; }
	}
}
