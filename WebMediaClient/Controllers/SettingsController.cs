using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		public ActionResult CreateSetting(int ownerID)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.OwnerID = ownerID;
			return View();
		}

		[HttpPost]
		public ActionResult CreateSetting(int ownerID, SettingViewModel settingModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/CreateSetting?OwnerID={0}", ownerID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var setting = SettingConverter.FromVisualToBasic(settingModel);
				//var createdSetting = await HttpClientBuilder<SettingModel>.PostAsync(setting, url, token);
				var createdSetting = Task.Run<SettingModel>(() => HttpClientBuilder<SettingModel>.PostAsync(setting, url, token)).Result;
				var viewModel = SettingConverter.FromBasicToVisual(createdSetting);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "CreateSetting");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> ChangePublicity(int settingID, string publicity)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/ChangePublicity?SettingID={0}&Publicity={1}", settingID, publicity);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<SettingModel>.PutEmptyAsync(url, token);
				return Json(new { Response = response }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpDelete]
		public ActionResult DeleteSetting(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/DeleteSetting?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<SettingModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "DeleteSetting");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetSettingByID(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/GetSettingByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var setting = await HttpClientBuilder<SettingModel>.GetAsync(url, token);
				var viewModel = SettingConverter.FromBasicToVisual(setting);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.ID = ID;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetSettingByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetSettingByOwnerIDAndType(int ownerID, string settingType)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/GetSettingByOwnerIDAndType?OwnerID={0}&Type={1}", ownerID, settingType);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var setting = await HttpClientBuilder<SettingModel>.GetAsync(url, token);
				var viewModel = SettingConverter.FromBasicToVisual(setting);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.OwnerID = ownerID;
				ViewBag.SettingType = settingType;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetSettingByOwnerIDAndType");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetSettingByOwnerIDAndTypeRaw(int ownerID, string settingType)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Setting/GetSettingByOwnerIDAndType?OwnerID={0}&Type={1}", ownerID, settingType);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var setting = await HttpClientBuilder<SettingModel>.GetAsync(url, token);
				return Json(new { Publicity = setting.Publicity }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetPendingFriends(int userID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetPendingFriends?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				var profiles = Task.Run<List<ProfileModel>>(() => HttpClientBuilder<ProfileModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<ProfileViewModel>();
				foreach (ProfileModel p in profiles)
				{
					viewModels.Add(ProfileConverter.FromBasicToVisual(p));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetPendingFriends");
				return View("Error", info);
			}
		}

		public ActionResult GetSubscribedTopics(int userID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/GetSubscribedTopics?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetSubscribedTopics");
				return View("Error", info);
			}
		}

		public ActionResult GetTopicsWithNewComments(int userID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/GetTopicsWithNewComments?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetTopicsWithNewComments");
				return View("Error", info);
			}
		}

		public ActionResult GetFirstLatestProfileActivity(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetLatestProfileActivity?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var activities = Task.Run<List<ActivityModel>>(() => HttpClientBuilder<ActivityModel>.GetListAsync(url, token)).Result;
				activities = activities.OrderByDescending(a => a.DateCreated).Take(10).ToList();
				var viewModels = new List<ActivityViewModel>();
				foreach (ActivityModel a in activities)
				{
					viewModels.Add(ActivityConverter.FromBasicToVisual(a));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetFirstLatestProfileActivity");
				return View("Error", info);
			}
		}

		public ActionResult GetLatestProfileActivity(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetLatestProfileActivity?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var activities = Task.Run<List<ActivityModel>>(() => HttpClientBuilder<ActivityModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<ActivityViewModel>();
				foreach (ActivityModel a in activities)
				{
					viewModels.Add(ActivityConverter.FromBasicToVisual(a));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetLatestProfileActivity");
				return View("Error", info);
			}
		}

		public ActionResult GetFirstLatestFriendsActivity(int userID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetLatestFriendsActivity?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var activities = Task.Run<List<ActivityModel>>(() => HttpClientBuilder<ActivityModel>.GetListAsync(url, token)).Result;
				activities = activities.OrderByDescending(a => a.DateCreated).Take(5).ToList();
				var viewModels = new List<ActivityViewModel>();
				foreach (ActivityModel a in activities)
				{
					viewModels.Add(ActivityConverter.FromBasicToVisual(a));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetFirstLatestFriendsActivity");
				return View("Error", info);
			}
		}

		public ActionResult GetLatestFriendsActivity(int userID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetLatestFriendsActivity?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var activities = Task.Run<List<ActivityModel>>(() => HttpClientBuilder<ActivityModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<ActivityViewModel>();
				foreach (ActivityModel a in activities)
				{
					viewModels.Add(ActivityConverter.FromBasicToVisual(a));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "GetLatestFriendsActivity");
				return View("Error", info);
			}
		}

		[HttpPut]
		public ActionResult SuspendUser(int sectionID, int? userID = null)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.SectionID = sectionID;
				ViewBag.UserID = userID;
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "SuspendUser");
				return View("Error", info);
			}
		}

		[HttpPut]
		public ActionResult ChangePosition(int sectionID, int? userID = null)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.SectionID = sectionID;
				ViewBag.UserID = userID;
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Settings", "ChangePosition");
				return View("Error", info);
			}
		}
	}
}