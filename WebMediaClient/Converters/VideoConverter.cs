using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class VideoConverter
	{
		public static VideoViewModel FromBasicToVisual(VideoModel videoModel)
		{
			var viewModel = new VideoViewModel
			{
				ID = videoModel.ID,
				Location = videoModel.Location,
				OwnerID = videoModel.OwnerID,
				DateCreated = videoModel.DateCreated,
				IsInterest = videoModel.IsInterest,
				IsProfile = videoModel.IsProfile,
				Rating = videoModel.Rating,
			};

			return viewModel;
		}

		public static VideoModel FromVisualToBasic(VideoViewModel viewModel)
		{
			var videoModel = new VideoModel
			{
				ID = viewModel.ID,
				Location = viewModel.Location,
				OwnerID = viewModel.OwnerID,
				DateCreated = viewModel.DateCreated,
				IsInterest = viewModel.IsInterest,
				IsProfile = viewModel.IsProfile,
				Rating = viewModel.Rating,
			};

			return videoModel;
		}
	}
}
