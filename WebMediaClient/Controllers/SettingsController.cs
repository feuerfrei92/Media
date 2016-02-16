using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMediaClient.Converters;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult GetPendingFriends(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetPendingFriends?UserID={0}", userID);
				//var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				var profiles = Task.Run<List<ProfileModel>>(() => HttpClientBuilder<ProfileModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<ProfileViewModel>();
				foreach (ProfileModel p in profiles)
				{
					viewModels.Add(ProfileConverter.FromBasicToVisual(p));
				}
				ViewBag.User = GlobalUser.User;
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetSubscribedTopics(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetSubscribedTopics?UserID={0}", userID);
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetTopicsWithNewComments(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetSubscribedTopics?UserID={0}", userID);
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}