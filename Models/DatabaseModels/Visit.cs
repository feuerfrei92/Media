using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Visit
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public int UserID { get; set; }
		[Required]
		public int TopicID { get; set; }
		[Required]
		public DateTime LastVisit { get; set; }
	}
}
