using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Interest
	{
		[Required]
		[Key]
		public int ID { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
