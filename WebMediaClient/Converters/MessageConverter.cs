using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class MessageConverter
	{
		public static MessageViewModel FromBasicToVisual(MessageModel messageModel)
		{
			var viewModel = new MessageViewModel
			{
				ID = messageModel.ID,
				SenderID = messageModel.SenderID,
				ReceiverID = messageModel.ReceiverID,
				Text = messageModel.Text,
				DateCreated = messageModel.DateCreated,
			};

			return viewModel;
		}

		public static MessageModel FromVisualToBasic(MessageViewModel viewModel)
		{
			var messageModel = new MessageModel
			{
				ID = viewModel.ID,
				SenderID = viewModel.SenderID,
				ReceiverID = viewModel.ReceiverID,
				Text = viewModel.Text,
				DateCreated = viewModel.DateCreated,
			};

			return messageModel;
		}
	}
}
