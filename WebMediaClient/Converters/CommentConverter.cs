using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class CommentConverter
	{
		public CommentViewModel FromBasicToVisual(CommentModel commentModel)
		{
			var viewModel = new CommentViewModel
			{
				ID = commentModel.ID,
				Name = commentModel.Name,
				Text = commentModel.Text,
				DateCreated = commentModel.DateCreated,
				DateModified = commentModel.DateModified
			};

			return viewModel;
		}

		public CommentModel FromVisualToBasic(CommentViewModel viewModel)
		{
			var commentModel = new CommentModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
				Text = viewModel.Text,
				DateCreated = viewModel.DateCreated,
				DateModified = viewModel.DateModified
			};

			return commentModel;
		}
	}
}
