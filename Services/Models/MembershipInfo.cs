using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
	public class MembershipInfo
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int SectionID { get; set; }
		public SectionRole Role { get; set; }
		public DateTime? SuspendedUntil { get; set; }
	}
}