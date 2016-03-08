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
using System.Web.Security;
using WebMediaClient.Converters;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Photo
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllAlbums(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Album/GetAllAlbums";
				var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				var viewModels = new List<AlbumViewModel>();
				foreach (AlbumModel a in albums)
				{
					viewModels.Add(AlbumConverter.FromBasicToVisual(a));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult CreateAlbum(int ownerID, bool isProfile, string token)
		{
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			ViewBag.IsProfile = isProfile;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateAlbum(int ownerID, bool isProfile, AlbumViewModel albumModel, string token)
		{
			string url = string.Format("http://localhost:8080/api/Album/CreateAlbum?UserID={0}", ownerID);
			var album = AlbumConverter.FromVisualToBasic(albumModel);
			var createdAlbum = await HttpClientBuilder<AlbumModel>.PostAsync(album, url, token);
			var	viewModel = AlbumConverter.FromBasicToVisual(createdAlbum);
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			ViewBag.IsProfile = isProfile;
			return View(viewModel);
		}

		public ActionResult DeleteAlbum(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/DeleteAlbum?ID={0}", ID);
				HttpClientBuilder<PhotoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetAlbumByID(int ID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumByID?ID={0}", ID);
				var album = await HttpClientBuilder<AlbumModel>.GetAsync(url, token);
				var viewModel = AlbumConverter.FromBasicToVisual(album);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetAlbumsForProfile(int profileID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForProfile?OwnerID={0}", profileID);
				//var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				var albums = Task.Run<List<AlbumModel>>(() => HttpClientBuilder<AlbumModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<AlbumViewModel>();
				foreach (AlbumModel a in albums)
				{
					viewModels.Add(AlbumConverter.FromBasicToVisual(a));
				}
				return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetAlbumsForProfileRaw(int profileID, string token)
		{
			//try
			//{
			string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForProfile?OwnerID={0}", profileID);
			var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
			return Json(albums, JsonRequestBehavior.AllowGet);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetAlbumsForInterest(int interestID, string token)
		{
			//try
			//{
			string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForInterest?OwnerID={0}", interestID);
			//var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
			var albums = Task.Run<List<AlbumModel>>(() => HttpClientBuilder<AlbumModel>.GetListAsync(url, token)).Result;
			var viewModels = new List<AlbumViewModel>();
			foreach (AlbumModel a in albums)
			{
				viewModels.Add(AlbumConverter.FromBasicToVisual(a));
			}
			return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> UpdateAlbumRating(int albumID, bool like, string token)
		{
			string url = string.Format("http://localhost:8080/api/Album/UpdateAlbumRating?AlbumID={0}&Like={1}", albumID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> GetAllPhotos(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Album/GetAllPhotos";
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
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
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
				string url = string.Format("http://localhost:8080/api/Album/CreatePhoto?UserID={0}", userID);
				var photo = PhotoConverter.FromVisualToBasicIn(photoModel);
				var createdPhoto = await HttpClientBuilder<PhotoModel>.PostAsync(photo, url, token);
				var viewModel = PhotoConverter.FromBasicToVisualIn(createdPhoto);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			else throw new ArgumentNullException(file.FileName);
		}

		public ActionResult DeletePhoto(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/DeletePhoto?ID={0}", ID);
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
				string url = string.Format("http://localhost:8080/api/Album/GetPhotoByID?ID={0}", ID);
				var photo = await HttpClientBuilder<PhotoModel>.GetAsync(url, token);
				return File(photo.Content, "image/jpeg");
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetPhotosForAlbum(int albumID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Album/GetPhotosForAlbum?AlbumID={0}", albumID);
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
			string url = string.Format("http://localhost:8080/api/Album/UpdateRating?PhotoID={0}&Like={1}", photoID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}
    }
}