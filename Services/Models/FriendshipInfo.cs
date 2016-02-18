using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class FriendshipInfo
	{
		public int ID { get; set; }
		public int UserID_1 { get; set; }
		public int UserID_2 { get; set; }
		public bool IsAccepted { get; set; }
	}
}