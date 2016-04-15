using Services.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

		public async Task<ActionResult> GetAllAlbums()
		{
			try
			{
				string url = "http://localhost:8080/api/Album/GetAllAlbums";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				var viewModels = new List<AlbumViewModel>();
				foreach (AlbumModel a in albums)
				{
					viewModels.Add(AlbumConverter.FromBasicToVisual(a));
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetAllAlbums");
				return View("Error", info);
			}
		}

		public ActionResult CreateAlbum(int ownerID, bool isProfile)
		{
			if (((UserModel)HttpContext.Session["currentUser"]).ID != ownerID && isProfile)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (!isProfile)
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
			}

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			ViewBag.IsProfile = isProfile;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateAlbum(int ownerID, bool isProfile, AlbumViewModel albumModel)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != ownerID && isProfile)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if(!isProfile)
				{
					if(Request.UrlReferrer == null)
						return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
				}

				string url = string.Format("http://localhost:8080/api/Album/CreateAlbum?UserID={0}", ownerID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var album = AlbumConverter.FromVisualToBasic(albumModel);
				album.IsProfile = isProfile;
				album.IsInterest = !isProfile;
				var createdAlbum = await HttpClientBuilder<AlbumModel>.PostAsync(album, url, token);
				var viewModel = AlbumConverter.FromBasicToVisual(createdAlbum);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.IsProfile = isProfile;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "CreateAlbum");
				return View("Error", info);
			}
		}

		public ActionResult DeleteAlbum(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Album/DeleteAlbum?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<PhotoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "DeleteAlbum");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetAlbumByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var album = await HttpClientBuilder<AlbumModel>.GetAsync(url, token);
				var viewModel = AlbumConverter.FromBasicToVisual(album);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.ID = ID;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetAlbumByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetAlbumByIDRaw(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var album = await HttpClientBuilder<AlbumModel>.GetAsync(url, token);
				return Json(new { ID = album.ID, Name = album.Name, OwnerID = album.OwnerID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetAlbumsForProfile(int profileID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForProfile?OwnerID={0}", profileID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				var albums = Task.Run<List<AlbumModel>>(() => HttpClientBuilder<AlbumModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<AlbumViewModel>();
				foreach (AlbumModel a in albums)
				{
					viewModels.Add(AlbumConverter.FromBasicToVisual(a));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetAlbumsForProfile");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetAlbumsForProfileRaw(int profileID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForProfile?OwnerID={0}", profileID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				return Json(albums, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetAlbumsForInterest(int interestID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetAlbumsForInterest?OwnerID={0}", interestID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var albums = await HttpClientBuilder<AlbumModel>.GetListAsync(url, token);
				var albums = Task.Run<List<AlbumModel>>(() => HttpClientBuilder<AlbumModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<AlbumViewModel>();
				foreach (AlbumModel a in albums)
				{
					viewModels.Add(AlbumConverter.FromBasicToVisual(a));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetAlbumsForInterest");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> UpdateAlbumRating(int albumID, bool like)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Album/UpdateAlbumRating?AlbumID={0}&Like={1}", albumID, like);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetAllPhotos()
		{
			try
			{
				string url = "http://localhost:8080/api/Album/GetAllPhotos";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var photos = await HttpClientBuilder<PhotoModel>.GetListAsync(url, token);
				var viewModels = new List<PhotoViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisual(p));
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetAllPhotos");
				return View("Error", info);
			}
		}

		public ActionResult CreatePhoto(int userID, int albumID)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			ViewBag.ID = albumID;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreatePhoto(int userID, int albumID, PhotoViewModel photoModel, HttpPostedFileBase file)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (file != null && file.ContentLength > 0)
			{
				string guid = Guid.NewGuid().ToString() + ".jpg";
				photoModel.Location = @"\UploadedFiles\Photos\ContentID=" + guid;
				photoModel.DateCreated = DateTime.Now;
				string url = string.Format("http://localhost:8080/api/Album/CreatePhoto?UserID={0}&AlbumID={1}", userID, albumID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var photo = PhotoConverter.FromVisualToBasic(photoModel);
				var createdPhoto = await HttpClientBuilder<PhotoModel>.PostAsync(photo, url, token);
				var viewModel = PhotoConverter.FromBasicToVisual(createdPhoto);
				file.SaveAs(Server.MapPath(@"\UploadedFiles\Photos\ContentID=" + guid));
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.ID = albumID;
				return View(viewModel);
			}
			else throw new ArgumentNullException(file.FileName);
		}

		public ActionResult DeletePhoto(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Album/DeletePhoto?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<PhotoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "DeletePhoto");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetPhotoByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetPhotoByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var photo = await HttpClientBuilder<PhotoModel>.GetAsync(url, token);
				var imageStream = System.IO.File.OpenRead(Server.MapPath(photo.Location));
				byte[] bytes = new byte[imageStream.Length];
				int allToRead = (int)imageStream.Length;
				int readSoFar = 0;
				while (allToRead > 0)
				{
					int n = imageStream.Read(bytes, readSoFar, allToRead);

					if (n == 0)
						break;

					readSoFar += n;
					allToRead -= n;
				}
				return File(bytes, "image/jpg");
				//var viewModel = PhotoConverter.FromBasicToVisual(photo);
				//return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetPhotoByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetPhotoByIDRaw(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetPhotoByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var photo = await HttpClientBuilder<PhotoModel>.GetAsync(url, token);
				return Json(new { ID = photo.ID, OwnerID = photo.AlbumID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetPhotosForAlbum(int albumID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Album/GetPhotosForAlbum?AlbumID={0}", albumID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var photos = await HttpClientBuilder<PhotoModel>.GetListAsync(url, token);
				var photos = Task.Run<List<PhotoModel>>(() => HttpClientBuilder<PhotoModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<PhotoViewModel>();
				foreach (PhotoModel p in photos)
				{
					viewModels.Add(PhotoConverter.FromBasicToVisual(p));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Album", "GetPhotosForAlbum");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateRating(int photoID, bool like)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Album/UpdateRating?PhotoID={0}&Like={1}", photoID, like);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}