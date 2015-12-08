using Services.Models;
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
		public ProfileViewModel FromBasicToVisual(ProfileModel profileModel)
		{
			var viewModel = new ProfileViewModel
			{
				ID = profileModel.ID,
				Age = profileModel.Age,
				Gender = profileModel.Gender,
				Name = profileModel.Name,
				Username = profileModel.Username,
			};

			return viewModel;
		}

		public ProfileModel FromVisualToBasic(ProfileViewModel viewModel)
		{
			var profileModel = new ProfileModel
			{
				ID = viewModel.ID,
				Age = viewModel.Age,
				Gender = viewModel.Gender,
				Name = viewModel.Name,
				Username = viewModel.Username,
			};

			return profileModel;
		}
	}
}
