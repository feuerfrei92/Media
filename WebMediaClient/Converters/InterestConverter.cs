﻿using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class InterestConverter
	{
		public static InterestViewModel FromBasicToVisual(InterestModel interestModel)
		{
			var viewModel = new InterestViewModel
			{
				ID = interestModel.ID,
				Name = interestModel.Name,
				AuthorID = interestModel.AuthorID,
				PictureID = interestModel.PictureID,
			};

			return viewModel;
		}

		public static InterestModel FromVisualToBasic(InterestViewModel viewModel)
		{
			var interestModel = new InterestModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
				AuthorID = viewModel.AuthorID,
				PictureID = viewModel.PictureID,
			};

			return interestModel;
		}
	}
}
