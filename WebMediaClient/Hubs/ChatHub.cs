using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Services.Models;
using WebMediaClient.Models;

namespace WebMediaClient.Chat
{
	public class ChatHub : Hub
	{
		public void Send(string userID, string message)
		{
			Clients.User(userID).broadcastMessage(message);
		}

		public override Task OnConnected()
		{
			string username = Context.QueryString["receiverUsername"];
			var loggedInUsers = (List<ChatUser>)HttpRuntime.Cache["loggedInUsers"];
			if (loggedInUsers != null)
			{
				var currentUser = loggedInUsers.Where(c => c.Username == username).FirstOrDefault();
				if (currentUser != null)
				{
					loggedInUsers.Remove(currentUser);
					currentUser.ConnectionID = Context.ConnectionId;
					loggedInUsers.Add(currentUser);
				}
				HttpRuntime.Cache["loggedInUsers"] = loggedInUsers;
			}

			return Clients.All.joined(Context.ConnectionId, loggedInUsers);
		}
	}
}