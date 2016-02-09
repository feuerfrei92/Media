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
		public static CommentViewModel FromBasicToVisual(CommentModel commentModel)
		{
			var viewModel = new CommentViewModel
			{
				ID = commentModel.ID,
				Name = commentModel.Name,
				Text = commentModel.Text,
				TopicID = commentModel.TopicID,
				AuthorID = commentModel.AuthorID,
				DateCreated = commentModel.DateCreated,
				DateModified = commentModel.DateModified
			};

			return viewModel;
		}

		public static CommentModel FromVisualToBasic(CommentViewModel viewModel)
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

        public static CommentCriteriaViewModel CriteriaFromBasicToVisual(CommentCriteria criteria)
        {
            var viewModel = new CommentCriteriaViewModel
            {
                AuthorID = criteria.AuthorID,
                DateCreatedFrom = criteria.DateCreatedFrom,
                DateCreatedTo = criteria.DateCreatedTo,
                DateModifiedFrom = criteria.DateModifiedFrom,
                DateModifiedTo = criteria.DateModifiedTo,
                Name = criteria.Name,
                TopicID = criteria.TopicID
            };

            return viewModel;
        }

        public static CommentCriteria CriteriaFromVisualToBasic(CommentCriteriaViewModel viewModel)
        {
            var criteria = new CommentCriteria
            {
                AuthorID = viewModel.AuthorID,
                DateCreatedFrom = viewModel.DateCreatedFrom,
                DateCreatedTo = viewModel.DateCreatedTo,
                DateModifiedFrom = viewModel.DateModifiedFrom,
                DateModifiedTo = viewModel.DateModifiedTo,
                Name = viewModel.Name,
                TopicID = viewModel.TopicID
            };

            return criteria;
        }
	}
}
