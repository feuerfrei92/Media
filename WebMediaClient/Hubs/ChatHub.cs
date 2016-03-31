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
		private readonly static ConnectionProvider _connections = 
            new ConnectionProvider();

		public void Send(string userID, string message)
        {
			Clients.Caller.broadcastMessage(message);
		    
			foreach (var connectionId in _connections.GetConnections(userID))
            {
                Clients.Client(connectionId).broadcastMessage(message);
            }
        }

        public override Task OnConnected()
        {
			string username = Context.QueryString["id"];

			_connections.Add(username, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
			string username = Context.QueryString["id"];

			_connections.Remove(username, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string username = Context.QueryString["id"];

            if (!_connections.GetConnections(username).Contains(Context.ConnectionId))
            {
                _connections.Add(username, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}