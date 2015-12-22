using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class VideoViewModel
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int OwnerID { get; set; }
		[Required]
		public string Location { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
	}
}
