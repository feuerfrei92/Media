using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class SectionConverter
	{
		public SectionViewModel FromBasicToVisual(SectionModel sectionModel)
		{
			var viewModel = new SectionViewModel
			{
				ID = sectionModel.ID,
				Name = sectionModel.Name,
			};

			return viewModel;
		}

		public SectionModel FromVisualToBasic(SectionViewModel viewModel)
		{
			var sectionModel = new SectionModel
			{
				ID = viewModel.ID,
				Name = viewModel.Name,
			};

			return sectionModel;
		}
	}
}
