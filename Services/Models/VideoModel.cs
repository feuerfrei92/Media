using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class VideoModel
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int OwnerID { get; set; }
		[Required]
		public string Location { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public bool IsProfile { get; set; }
		public bool IsInterest { get; set; }
		public int Rating { get; set; }
	}
}
