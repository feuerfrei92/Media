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
		public static TopicViewModel FromBasicToVisual(TopicModel topicModel)
		{
			var viewModel = new TopicViewModel
			{
				ID = topicModel.ID,
				Name = topicModel.Name,
				SectionID = topicModel.SectionID,
				AuthorID = topicModel.AuthorID,
				DateCreated = topicModel.DateCreated,
				DateModified = topicModel.DateModified
			};

			return viewModel;
		}

		public static TopicModel FromVisualToBasic(TopicViewModel viewModel)
		{
			var topicModel = new TopicModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
				SectionID = viewModel.SectionID, 
				AuthorID = viewModel.AuthorID,
				DateCreated = viewModel.DateCreated,
				DateModified = viewModel.DateModified
			};

			return topicModel;
		}

        public static TopicCriteriaViewModel CriteriaFromBasicToVisual(TopicCriteria criteria)
        {
            var viewModel = new TopicCriteriaViewModel
            {
                AuthorID = criteria.AuthorID,
                DateCreatedFrom = criteria.DateCreatedFrom,
                DateCreatedTo = criteria.DateCreatedTo,
                DateModifiedFrom = criteria.DateModifiedFrom,
                DateModifiedTo = criteria.DateModifiedTo,
                Name = criteria.Name,
                SectionID = criteria.SectionID
            };

            return viewModel;
        }

        public static TopicCriteria CriteriaFromVisualToBasic(TopicCriteriaViewModel viewModel)
        {
            var criteria = new TopicCriteria
            {
                AuthorID = viewModel.AuthorID,
                DateCreatedFrom = viewModel.DateCreatedFrom,
                DateCreatedTo = viewModel.DateCreatedTo,
                DateModifiedFrom = viewModel.DateModifiedFrom,
                DateModifiedTo = viewModel.DateModifiedTo,
                Name = viewModel.Name,
                SectionID = viewModel.SectionID
            };

            return criteria;
        }
	}
}
