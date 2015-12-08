using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class CommentModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		[Required]
		public string Text { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
