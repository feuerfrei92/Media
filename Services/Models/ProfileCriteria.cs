using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class ProfileCriteria
	{
		public string Name { get; set; }
		public int? MinimumAge { get; set; }
		public int? MaximumAge { get; set; }
		public GenderCriterion Gender { get; set; }
	}
}
