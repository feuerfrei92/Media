using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMediaClient.Converters;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllVideos()
		{
			try
			{
				string url = "http://localhost:8080/api/Video/GetAllVideos";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var videos = await HttpClientBuilder<VideoModel>.GetListAsync(url, token);
				var viewModels = new List<VideoViewModel>();
				foreach (VideoModel v in videos)
				{
					viewModels.Add(VideoConverter.FromBasicToVisual(v));
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Video", "GetAllVideos");
				return View("Error", info);
			}
		}

		public ActionResult CreateVideo(int userID)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateVideo(int userID, VideoViewModel videoModel, HttpPostedFileBase file)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (file != null && file.ContentLength > 0)
				{
					string guid = Guid.NewGuid().ToString() + ".swf";
					videoModel.Location = @"\UploadedFiles\Videos\ContentID=" + guid;
					videoModel.DateCreated = DateTime.Now;
					string url = string.Format("http://localhost:8080/api/Video/CreateVideo?UserID={0}", userID);
					string token = "";
					if (HttpContext.Session["token"] != null)
						token = HttpContext.Session["token"].ToString();
					else
						token = null;
					var video = VideoConverter.FromVisualToBasic(videoModel);
					var createdVideo = await HttpClientBuilder<VideoModel>.PostAsync(video, url, token);
					var viewModel = VideoConverter.FromBasicToVisual(createdVideo);
					file.SaveAs(Server.MapPath(@"\UploadedFiles\Videos\ContentID=" + guid));
					ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
					return View(viewModel);
				}
				else throw new ArgumentNullException(file.FileName);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Video", "CreateVideo");
				return View("Error", info);
			}
		}

		public ActionResult DeleteVideo(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Video/DeleteVideo?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<VideoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Video", "DeleteVideo");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetVideoByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/GetVideoByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var video = await HttpClientBuilder<VideoModel>.GetAsync(url, token);
				var viewModel = VideoConverter.FromBasicToVisual(video);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Video", "GetVideoByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetVideoByIDRaw(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/GetVideoByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var video = await HttpClientBuilder<VideoModel>.GetAsync(url, token);
				return Json(new { ID = video.ID, OwnerID = video.OwnerID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetVideosForOwner(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/GetVideosForOwner?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var videos = await HttpClientBuilder<VideoModel>.GetListAsync(url, token);
				var viewModels = new List<VideoViewModel>();
				foreach (VideoModel v in videos)
				{
					viewModels.Add(VideoConverter.FromBasicToVisual(v));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.UserID = userID;
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Video", "GetVideosForOwner");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> UpdateRating(int videoID, bool like)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Video/UpdateRating?VideoID={0}&Like={1}", videoID, like);
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