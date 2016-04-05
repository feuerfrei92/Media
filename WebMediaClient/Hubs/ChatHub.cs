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
		//private readonly static ConnectionProvider _connections = 
		//	new ConnectionProvider();

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


			//foreach (var connectionId in _connections.GetConnections(userID))
			//{
			//	Clients.Client(connectionId).broadcastMessage(message);
			//}
		}

		public override Task OnConnected()
		{
			WebMediaClient.ChatUser.ChatUserModel chatUser = ChatUser.onlineUsers.Where(c => c.SessionID == HttpContext.Current.Request.Cookies["ASP.NET_SessionId"].Value).FirstOrDefault();
			//ChatUser.RemoveOnlineUser("", chatUser.User.ID);
			chatUser.ConnectionID = Context.ConnectionId;
			//ChatUser.AddOnlineUser(chatUser.User, chatUser.ConnectionID, chatUser.SessionID);
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

		//	public override Task OnConnected()
		//	{
		//		string username = Context.QueryString["id"];

		//		_connections.Add(username, Context.ConnectionId);

		//		return base.OnConnected();
		//	}

		//	public override Task OnDisconnected(bool stopCalled)
		//	{
		//		string username = Context.QueryString["id"];

		//		_connections.Remove(username, Context.ConnectionId);

		//		return base.OnDisconnected(stopCalled);
		//	}

		//	public override Task OnReconnected()
		//	{
		//		string username = Context.QueryString["id"];

		//		if (!_connections.GetConnections(username).Contains(Context.ConnectionId))
		//		{
		//			_connections.Add(username, Context.ConnectionId);
		//		}

		//		return base.OnReconnected();
		//	}
		//}
	}
}