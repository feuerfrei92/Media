using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Album
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public int OwnerID { get; set; }
		public bool IsProfile { get; set; }
		public bool IsInterest { get; set; }
		public int Size { get; set; }
		public int Rating { get; set; }
	}
}
