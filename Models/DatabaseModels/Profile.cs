using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Profile
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int UserID { get; set; }
		[Required]
		public string Username { get; set; }
		public string Name { get; set; }
		public int? Age { get; set; }
		public Gender Gender { get; set; }
	}
}
