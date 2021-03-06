﻿using Services.Models;
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
		public static PhotoViewModel FromBasicToVisual(PhotoModel photoModel)
		{
			var viewModel = new PhotoViewModel
			{
				ID = photoModel.ID,
				Location = photoModel.Location,
				AlbumID = photoModel.AlbumID,
				DateCreated = photoModel.DateCreated,
				Rating = photoModel.Rating
			};

			return viewModel;
		}

		public static PhotoModel FromVisualToBasic(PhotoViewModel viewModel)
		{
			var PhotoModel = new PhotoModel
			{
				ID = viewModel.ID,
				Location = viewModel.Location,
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
