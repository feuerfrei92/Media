using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class ActivityConverter
	{
		public static ActivityViewModel FromBasicToVisual(ActivityModel activityModel)
		{
			var viewModel = new ActivityViewModel
			{
				Action = activityModel.Action,
				ActionID = activityModel.ActionID,
				DateCreated = activityModel.DateCreated,
			};

			viewModel.Author = new UserModel
			{
				ID = activityModel.Author.ID,
				Username = activityModel.Author.Username,
			};

			return viewModel;
		}

		public static ActivityModel FromVisualToBasic(ActivityViewModel viewModel)
		{
			var activityModel = new ActivityModel
			{
				Action = viewModel.Action,
				ActionID = viewModel.ActionID,
				DateCreated = viewModel.DateCreated,
			};

			activityModel.Author = new UserModel
			{
				ID = viewModel.Author.ID,
				Username = viewModel.Author.Username,
			};

			return activityModel;
		}
	}
}
