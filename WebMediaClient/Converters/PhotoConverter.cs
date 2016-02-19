using Services.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMediaClient.Models;

namespace WebMediaClient.Converters
{
	public class PhotoConverter
	{
		public static PhotoInViewModel FromBasicToVisualIn(PhotoModel photoModel)
		{
			var content = byteArrayToImage(photoModel.Content);

			var viewModel = new PhotoInViewModel
			{
				ID = photoModel.ID,
				Content = content,
				AlbumID = photoModel.AlbumID,
				DateCreated = photoModel.DateCreated,
				Rating = photoModel.Rating
			};

			return viewModel;
		}

		public static PhotoModel FromVisualToBasicIn(PhotoInViewModel viewModel)
		{
			var content = imageToByteArray(viewModel.Content);

			var PhotoModel = new PhotoModel
			{
				ID = viewModel.ID,
				Content = content,
				AlbumID = viewModel.AlbumID,
				DateCreated = viewModel.DateCreated,
				Rating = viewModel.Rating
			};

			return PhotoModel;
		}

		public static PhotoOutViewModel FromBasicToVisualOut(PhotoModel photoModel)
		{
			var viewModel = new PhotoOutViewModel
			{
				ID = photoModel.ID,
				Content = photoModel.Content,
				AlbumID = photoModel.AlbumID,
				DateCreated = photoModel.DateCreated,
				Rating = photoModel.Rating
			};

			return viewModel;
		}

		public static PhotoModel FromVisualToBasicOut(PhotoOutViewModel viewModel)
		{
			var PhotoModel = new PhotoModel
			{
				ID = viewModel.ID,
				Content = viewModel.Content,
				AlbumID = viewModel.AlbumID,
				DateCreated = viewModel.DateCreated,
				Rating = viewModel.Rating
			};

			return PhotoModel;
		}

		private static byte[] imageToByteArray(System.Drawing.Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, imageIn.RawFormat);
			return ms.ToArray();
		}

		private static Image byteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;
		}
	}
}
