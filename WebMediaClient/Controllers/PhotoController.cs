using Services.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMediaClient.Converters;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllPhotos(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Photo/GetAllPhotos";
				var photos = await HttpClientBuilder<PhotoModel>.GetListAsync(url, token);
				var viewModels = new List<PhotoOutViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisualOut(p));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult CreatePhoto(int userID, string token)
		{
			ViewBag.User = GlobalUser.User;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreatePhoto(int userID, PhotoInViewModel photoModel, HttpPostedFileBase file, string token)
		{
			if (file != null && file.ContentLength > 0)
			{
				Image image = Image.FromStream(file.InputStream, true, true);
				photoModel.Content = image;
				photoModel.DateCreated = DateTime.Now;
				string url = string.Format("http://localhost:8080/api/Photo/CreatePhoto?UserID={0}", userID);
				var photo = PhotoConverter.FromVisualToBasicIn(photoModel);
				var createdPhoto = await HttpClientBuilder<PhotoModel>.PostAsync(photo, url, token);
				var viewModel = PhotoConverter.FromBasicToVisualIn(createdPhoto);
				ViewBag.User = GlobalUser.User;
				return View(viewModel);
			}
			else throw new ArgumentNullException(file.FileName);
		}

		public ActionResult DeletePhoto(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Photo/DeletePhoto?ID={0}", ID);
				HttpClientBuilder<PhotoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetPhotoByID(int ID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Photo/GetPhotoByID?ID={0}", ID);
				var photo = await HttpClientBuilder<PhotoModel>.GetAsync(url, token);
				return File(photo.Content, "image/jpeg");
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetPhotosForOwner(int ownerID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Photo/GetPhotosForOwner?OwnerID={0}", ownerID);
				//var photos = await HttpClientBuilder<PhotoModel>.GetListAsync(url, token);
				var photos = Task.Run<List<PhotoModel>>(() => HttpClientBuilder<PhotoModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<PhotoOutViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisualOut(p));
				}
				return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> UpdateRating(int photoID, bool like, string token)
		{
			string url = string.Format("http://localhost:8080/api/Photo/UpdateRating?PhotoID={0}&Like={1}", photoID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}
    }
}