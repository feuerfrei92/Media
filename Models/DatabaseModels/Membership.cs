using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Membership
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public int UserID { get; set; }
		[Required]
		public int SectionID { get; set; }
		public bool IsAccepted { get; set; }
		[Required]
		public SectionRole Role { get; set; }
		public DateTime? SuspendedUntil { get; set; }
		public bool Anonymous { get; set; }
	}
}
