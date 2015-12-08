using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Topic
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int SectionID { get; set; }
		[Required]
		public int AuthorID { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
