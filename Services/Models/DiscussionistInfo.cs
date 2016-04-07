using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class DiscussionistInfo
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int DiscussionID { get; set; }
		[Required]
		public int UserID { get; set; }
	}
}
