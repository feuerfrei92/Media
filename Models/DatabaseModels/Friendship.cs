using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Friendship
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public int UserID_1 { get; set; }
		[Required]
		public int UserID_2 { get; set; }
	}
}
