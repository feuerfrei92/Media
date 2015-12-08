using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient.Models
{
	public class UserViewModel
	{
		public int ID { get; set; }
		[Required]
		public string Username { get; set; }
	}
}
