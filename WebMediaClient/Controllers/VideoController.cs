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
				return View("Error");
			}
		}

		public async Task<ActionResult> CreateVideo(int userID, VideoViewModel VideoModel)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Video/CreateVideo?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var video = VideoConverter.FromVisualToBasic(VideoModel);
				var createdVideo = await HttpClientBuilder<VideoModel>.PutAsync(video, url, token);
				var viewModel = VideoConverter.FromBasicToVisual(createdVideo);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				return View("Error");
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
				return View("Error");
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
				return View("Error");
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
				return View();
			}
			catch (Exception ex)
			{
				return View("Error");
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