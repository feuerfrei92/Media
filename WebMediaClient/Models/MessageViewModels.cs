using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class MessageViewModel
	{
		public int ID { get; set; }
		public int SenderID { get; set; }
		public int ReceiverID { get; set; }
		[Required]
		public string Text { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
	}
}
