﻿using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class ProfileConverter
	{
		public static ProfileViewModel FromBasicToVisual(ProfileModel profileModel)
		{
			var viewModel = new ProfileViewModel
			{
				ID = profileModel.ID,
				UserID = profileModel.UserID,
				Age = profileModel.Age,
				Gender = profileModel.Gender,
				Name = profileModel.Name,
				Username = profileModel.Username,
				PictureID = profileModel.PictureID,
			};

			return viewModel;
		}

		public static ProfileModel FromVisualToBasic(ProfileViewModel viewModel)
		{
			var profileModel = new ProfileModel
			{
				ID = viewModel.ID,
				UserID = viewModel.UserID,
				Age = viewModel.Age,
				Gender = viewModel.Gender,
				Name = viewModel.Name,
				Username = viewModel.Username,
				PictureID = viewModel.PictureID,
			};

			return profileModel;
		}

        public static ProfileCriteriaViewModel CriteriaFromBasicToVisual(ProfileCriteria criteria)
        {
            var viewModel = new ProfileCriteriaViewModel
            {
                Gender = criteria.Gender,
                MaximumAge = criteria.MaximumAge,
                MinimumAge = criteria.MinimumAge,
                Name = criteria.Name
            };

            return viewModel;
        }

        public static ProfileCriteria CriteriaFromVisualToBasic(ProfileCriteriaViewModel viewModel)
        {
            var criteria = new ProfileCriteria
            {
                Gender = viewModel.Gender,
                MaximumAge = viewModel.MaximumAge,
                MinimumAge = viewModel.MinimumAge,
                Name = viewModel.Name
            };

            return criteria;
        }
	}
}
