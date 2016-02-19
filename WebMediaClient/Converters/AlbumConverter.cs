using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class AlbumConverter
	{
		public static AlbumViewModel FromBasicToVisual(AlbumModel albumModel)
		{
			var viewModel = new AlbumViewModel
			{
				ID = albumModel.ID,
				Name = albumModel.Name,
				OwnerID = albumModel.OwnerID,
				IsInterest = albumModel.IsInterest,
				IsProfile = albumModel.IsProfile,
				Size = albumModel.Size,
				Rating = albumModel.Rating,
			};

			return viewModel;
		}

		public static AlbumModel FromVisualToBasic(AlbumViewModel viewModel)
		{
			var albumModel = new AlbumModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
				OwnerID = viewModel.OwnerID,
				IsInterest = viewModel.IsInterest,
				IsProfile = viewModel.IsProfile,
				Size = viewModel.Size,
				Rating = viewModel.Rating,
			};

			return albumModel;
		}
	}
}
