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
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllVideos(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Video/GetAllVideos";
				var videos = await HttpClientBuilder<VideoModel>.GetListAsync(url, token);
				var viewModels = new List<VideoViewModel>();
				foreach (VideoModel v in videos)
				{
					viewModels.Add(VideoConverter.FromBasicToVisual(v));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateVideo(int userID, VideoViewModel VideoModel, string token)
		{
			string url = string.Format("http://localhost:8080/api/Video/CreateVideo?UserID={0}", userID);
			var video = VideoConverter.FromVisualToBasic(VideoModel);
			var createdVideo = await HttpClientBuilder<VideoModel>.PutAsync(video, url, token);
			var viewModel = VideoConverter.FromBasicToVisual(createdVideo);
			return View(viewModel);
		}

		public ActionResult DeleteVideo(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/DeleteVideo?ID={0}", ID);
				HttpClientBuilder<VideoModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetVideoByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/GetVideoByID?ID={0}", ID);
				var video = await HttpClientBuilder<VideoModel>.GetAsync(url, token);
				var viewModel = VideoConverter.FromBasicToVisual(video);
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetVideosForOwner(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Video/GetVideosForOwner?UserID={0}", userID);
				var videos = await HttpClientBuilder<VideoModel>.GetListAsync(url, token);
				var viewModels = new List<VideoViewModel>();
				foreach (VideoModel v in videos)
				{
					viewModels.Add(VideoConverter.FromBasicToVisual(v));
				}
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> UpdateRating(int videoID, bool like, string token)
		{
			string url = string.Format("http://localhost:8080/api/Video/UpdateRating?VideoID={0}&Like={1}", videoID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}
    }
}