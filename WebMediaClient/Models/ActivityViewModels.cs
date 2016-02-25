using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class ActivityViewModel
	{
		public int ActionID { get; set; }
		public UserModel Author { get; set; }
		public string Action { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
