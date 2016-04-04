using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMediaClient
{
	public static class ChatUser
	{
		public class ChatUserModel
		{
			public UserModel User { get; set; }
			public string ConnectionID { get; set; }
			public string SessionID { get; set; }
		}

		public static List<ChatUserModel> onlineUsers = new List<ChatUserModel>();

		public static void AddOnlineUser(UserModel user, string connectionID, string sessionID)
		{
			var chatUser = new ChatUserModel
			{
				User = user,
				ConnectionID = connectionID,
				SessionID = sessionID,
			};
			onlineUsers.Add(chatUser);
		}

		public static void RemoveOnlineUser(string connectionID, int userID)
		{
			var user = (ChatUserModel)onlineUsers.Where(u => u.ConnectionID == connectionID && u.User.ID == userID);
			onlineUsers.Remove(user);
		}
	}
}
