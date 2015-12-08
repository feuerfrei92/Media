using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class TopicConverter
	{
		public TopicViewModel FromBasicToVisual(TopicModel topicModel)
		{
			var viewModel = new TopicViewModel
			{
				ID = topicModel.ID,
				Name = topicModel.Name,
				DateCreated = topicModel.DateCreated,
				DateModified = topicModel.DateModified
			};

			return viewModel;
		}

		public TopicModel FromVisualToBasic(TopicViewModel viewModel)
		{
			var topicModel = new TopicModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
				DateCreated = viewModel.DateCreated,
				DateModified = viewModel.DateModified
			};

			return topicModel;
		}
	}
}
