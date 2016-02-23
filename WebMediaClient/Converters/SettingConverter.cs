using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class SettingConverter
	{
		public static SettingViewModel FromBasicToVisual(SettingModel settingModel)
		{
			var viewModel = new SettingViewModel
			{
				ID = settingModel.ID,
				OwnerID = settingModel.OwnerID,
				OwnerType = (SettingType)Enum.Parse(typeof(SettingType), settingModel.OwnerType),
				Publicity = (PublicityType)Enum.Parse(typeof(PublicityType), settingModel.Publicity),
			};

			return viewModel;
		}

		public static SettingModel FromVisualToBasic(SettingViewModel viewModel)
		{
			var settingModel = new SettingModel
			{
				ID = viewModel.ID,
				OwnerID = viewModel.OwnerID,
				OwnerType = viewModel.OwnerType.ToString(),
				Publicity = viewModel.Publicity.ToString(),
			};

			return settingModel;
		}
	}
}
