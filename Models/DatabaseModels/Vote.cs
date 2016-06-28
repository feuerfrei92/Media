using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DatabaseModels
{
	public class Vote
	{
		[Required]
		public int ID { get; set; }
		[Required]
		public int TargetID { get; set; }
		[Required]
		public int VoterID { get; set; }
		[Required]
		public string Type { get; set; }
	}
}
