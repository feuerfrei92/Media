using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class ProfileCriteria
	{
		[RegularExpression("^[a-zA-Z]*$")]
		public string Name { get; set; }
		[Range(14, 100)]
		public int? MinimumAge { get; set; }
		[Range(14, 100)]
		public int? MaximumAge { get; set; }
		public string Gender { get; set; }
	}
}
