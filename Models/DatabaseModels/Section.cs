using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	public class Section
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
		public int? ParentSectionID { get; set; }
	}
}
