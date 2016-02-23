using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Setting
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int OwnerID { get; set; }
		[Required]
		public string OwnerType { get; set; }
		[Required]
		public string Publicity { get; set; }
	}
}
