using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
	public class DiscussionModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public Guid DiscussionGuid { get; set; }
	}
}
