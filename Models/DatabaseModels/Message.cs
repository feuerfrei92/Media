using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Message
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public int SenderID { get; set; }
		public int ReceiverID { get; set; }
		[Required]
		public string Text { get; set; }
		[Required]
		public DateTime DateCreated { get; set; }
		public int DiscussionID { get; set; }
		public bool IsRead { get; set; }
	}
}
