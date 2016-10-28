using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebMediaClient
{
	public static class ChatUser
	{
		public class ChatUserModel
		{
			public UserModel User { get; set; }
			public string ConnectionID { get; set; }
			public string SessionID { get; set; }
			public List<DiscussionModel> Groups { get; set; }
		}

		public static List<ChatUserModel> onlineUsers = new List<ChatUserModel>();

		public static void AddOnlineUser(UserModel user, string connectionID, string sessionID, List<DiscussionModel> groups)
		{
            if (!onlineUsers.Exists(u => u.User.ID == user.ID))
            { 
			    var chatUser = new ChatUserModel
			    {
				    User = user,
				    ConnectionID = connectionID,
				    SessionID = sessionID,
				    Groups = groups,
			    };
			    onlineUsers.Add(chatUser);
            }
            HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value = sessionID;
        }

		public static void RemoveOnlineUser(int userID)
		{
            var user = (ChatUserModel)onlineUsers.Where(u => u.User.ID == userID).FirstOrDefault(); ;
			onlineUsers.Remove(user);
		}
	}
}
