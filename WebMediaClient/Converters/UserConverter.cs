using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class UserConverter
	{
		public static UserViewModel FromBasicToVisual(UserModel userModel)
		{
			var viewModel = new UserViewModel
			{
				ID = userModel.ID,
				Username = userModel.Username,
			};

			return viewModel;
		}

		public static UserModel FromVisualToBasic(UserViewModel viewModel)
		{
			var userModel = new UserModel
			{
				ID = viewModel.ID,
				Username = viewModel.Username,
			};

			return userModel;
		}
	}
}
