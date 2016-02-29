using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class TopicModel
	{
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		public int SectionID { get; set; }
		public int AuthorID { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
