using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class InterestViewModel
	{
		public int ID { get; set; }
		[Required]
		[RegularExpression("^[a-zA-Z0-9]*$")]
		[Display(ResourceType = typeof(Resources), Name = "Name")]
		public string Name { get; set; }
		public int AuthorID { get; set; }
		public int? PictureID { get; set; }
	}
}
