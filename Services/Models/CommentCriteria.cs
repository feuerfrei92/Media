using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class CommentCriteria
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
