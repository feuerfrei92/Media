using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Services.Models;
using WebMediaClient.Models;
using WebMediaClient.Connections;

namespace WebMediaClient.Chat
{
	public class ChatHub : Hub
	{
		public void Send(string senderID, string receiverID, string message)
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.User.ID.ToString() == receiverID).FirstOrDefault();
			if (chatUser != null)
			{
				string connectionID = chatUser.ConnectionID;
				Clients.Caller.broadcastMessage(message, senderID, receiverID, senderID);
				Clients.Client(connectionID).broadcastMessage(message, receiverID, senderID, senderID);
			}
			else
				Clients.Caller.broadcastMessage(message, senderID, receiverID, senderID);
		}

		public void SendToGroup(string senderID, string groupName, string message)
		{
			Clients.Group(groupName).broadcastGroupMessage(message, senderID, groupName);
		}

		public override Task OnConnected()
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.SessionID == HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value).FirstOrDefault();
			chatUser.ConnectionID = Context.ConnectionId;
			return Clients.All.joined(chatUser.User.ID.ToString());
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.SessionID == HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value).FirstOrDefault();
			ChatUser.RemoveOnlineUser("", chatUser.User.ID);
			return Clients.All.left(chatUser.User.ID.ToString());
		}

		public void GetAllOnlineUsers()
		{
			Clients.Caller.onlineStatus(ChatUser.onlineUsers.Select(c => c.User.ID.ToString()).ToList());
		}

		public void LoadAllGroups()
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.SessionID == HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value).FirstOrDefault();
			Clients.Caller.loadGroups(chatUser.Groups.Select(g => g.DiscussionGuid.ToString()).ToList());
		}

		public void CreateGroup(string discussionGuid)
		{
			string groupName = null;
			if (string.IsNullOrEmpty(discussionGuid))
				groupName = Guid.NewGuid().ToString();
			else
                groupName = discussionGuid;
            Groups.Add(Context.ConnectionId, groupName);
			Clients.Group(groupName).openWindow(groupName);
		}

		public void AddGroupMember(string memberID, string groupName)
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.User.ID.ToString() == memberID).FirstOrDefault();
			if (chatUser != null)
			{
				string connectionID = chatUser.ConnectionID;
				Groups.Add(connectionID, groupName);
				Clients.Client(connectionID).openWindow(groupName);
			}
			else
			{
				string alertMessage = "This user is currently offline";
				Clients.Caller.showWarning(alertMessage);
			}
		}
	}
}