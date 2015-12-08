using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient
{
	public class TokenInfo
	{
		public string Access_Token { get; set; }
		public string Token_Type { get; set; }
		public long Expires_At { get; set; }
		public string Username { get; set; }
		public DateTime Issued { get; set; }
		public DateTime Expired { get; set; }
	}
}
