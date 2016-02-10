using Services.Models;
using System;
using System.Collections.Generic;
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
				var viewModels = new List<PhotoViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisual(p));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreatePhoto(int userID, PhotoViewModel photoModel, string token)
		{
			string url = string.Format("http://localhost:8080/api/Photo/CreatePhoto?UserID={0}", userID);
			var photo = PhotoConverter.FromVisualToBasic(photoModel);
			var createdPhoto = await HttpClientBuilder<PhotoModel>.PutAsync(photo, url, token);
			var viewModel = PhotoConverter.FromBasicToVisual(createdPhoto);
			return View(viewModel);
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
			try
			{
				string url = string.Format("http://localhost:8080/api/Photo/GetPhotoByID?ID={0}", ID);
				var photo = await HttpClientBuilder<PhotoModel>.GetAsync(url, token);
				var viewModel = PhotoConverter.FromBasicToVisual(photo);
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetPhotosForOwner(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Photo/GetPhotosForOwner?UserID={0}", userID);
				var photos = await HttpClientBuilder<PhotoModel>.GetListAsync(url, token);
				var viewModels = new List<PhotoViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisual(p));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> UpdateRating(int photoID, bool like, string token)
		{
			string url = string.Format("http://localhost:8080/api/Photo/UpdateRating?PhotoID={0}&Like={1}", photoID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}
    }
}