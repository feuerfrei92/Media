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
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllProfiles()
		{
			try
			{
				string url = "http://localhost:8080/api/Profile/GetAllProfiles";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
                var viewModels = new List<ProfileViewModel>();
                foreach (ProfileModel p in profiles)
                {
                    viewModels.Add(ProfileConverter.FromBasicToVisual(p));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetAllProfiles");
				return View("Error", info);
			}
		}

		public ActionResult CreateProfile(int userID)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateProfile(int userID, ProfileViewModel profileModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/CreateProfile?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = ProfileConverter.FromVisualToBasic(profileModel);
				var createdProfile = await HttpClientBuilder<ProfileModel>.PostAsync(profile, url, token);
				var viewModel = ProfileConverter.FromBasicToVisual(createdProfile);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "CreateProfile");
				return View("Error", info);
			}
		}

		[HttpPost]
		public async Task<ActionResult> AddFriend(int userID, int friendID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/AddFriend?UserID={0}&FriendID={1}", userID, friendID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPut]
		public async Task<ActionResult> AcceptFriendship(int userID, int friendID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/AcceptFriendship?UserID={0}&FriendID={1}", userID, friendID);
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

		public ActionResult UpdateProfile()
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateProfile(int ID, ProfileViewModel profileModel)
		{
            try
            {
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).Username != profileModel.Username)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                string url = string.Format("http://localhost:8080/api/Profile/UpdateProfile?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
                var profile = ProfileConverter.FromVisualToBasic(profileModel);
                var updatedProfile = await HttpClientBuilder<ProfileModel>.PutAsync(profile, url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(updatedProfile);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return RedirectToAction("Index", "Home");
            }
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "UpdateProfile");
				return View("Error", info);
			}
		}

		public ActionResult DeleteProfile(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/DeleteProfile?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<ProfileModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "DeleteProfile");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetProfileByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.ID = ID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetProfileByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetProfileByIDRaw(int ID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender, PictureID = profile.PictureID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetProfileByUserID(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.UserID = userID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
                return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetProfileByUserID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetProfileByUserIDRaw(int userID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender, PictureID = profile.PictureID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetProfileByUsernameRaw(string username)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender, PictureID = profile.PictureID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetProfileByUsername(string username)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.Username = username;
                return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetProfileByUsername");
				return View("Error", info);
			}
		}

		public ActionResult SearchProfilesByCriteria()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SearchProfilesByCriteria(ProfileCriteriaViewModel criteria)
		{
			try
			{
				string url = "http://localhost:8080/api/Profile/SearchProfilesByCriteria";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profileCriteria = ProfileConverter.CriteriaFromVisualToBasic(criteria);
				var profiles = await HttpClientBuilder<ProfileCriteria>.GetListAsync<ProfileModel>(profileCriteria, url, token);
				var viewModels = new List<ProfileViewModel>();
				foreach (ProfileModel p in profiles)
				{
					viewModels.Add(ProfileConverter.FromBasicToVisual(p));
				}
				TempData["viewModels"] = viewModels;
				return RedirectToAction("DisplayProfileSearchResults");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "SearchProfilesByCriteria");
				return View("Error", info);
			}
		}

		public ActionResult DisplayProfileSearchResults()
		{
			try
			{
				List<ProfileViewModel> viewModels = new List<ProfileViewModel>();
				if (TempData["viewModels"] != null)
					viewModels = (List<ProfileViewModel>)TempData["viewModels"];
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				TempData["viewModels"] = viewModels;
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "DisplayProfileSearchResults");
				return View("Error", info);
			}
		}

		public ActionResult GetAllFriends(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
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
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetAllFriends");
				return View("Error", info);
			}
		}

		public ActionResult PickFriends(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
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
				IEnumerable<SelectListItem> membersList =
					from vm in viewModels
					select new SelectListItem
					{
						Value = vm.ID.ToString(),
						Text = vm.Name,
					};

				ViewBag.Friends = membersList.ToList();
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "PickFriends");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetCommonFriends(int userID, int targetID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetCommonFriends?UserID={0}&TargetID={1}", userID, targetID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				return Json(new { Count = profiles.Count }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetFriendship(int userID, int friendID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetFriendship?UserID={0}&FriendID={1}", userID, friendID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var friendship = await HttpClientBuilder<FriendshipInfo>.GetAsync(url, token);
				if (friendship == null)
					return HttpNotFound("No friendship");
				else return Json(new { ID = friendship.ID, UserID_1 = friendship.UserID_1, UserID_2 = friendship.UserID_2, IsAccepted = friendship.IsAccepted }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public async Task<ActionResult> CreateMessage(int senderID, int receiverID, MessageViewModel messageModel)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != senderID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/CreateMessage?SenderID={0}&ReceiverID={1}", senderID, receiverID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var message = MessageConverter.FromVisualToBasic(messageModel);
				var createdMessage = await HttpClientBuilder<MessageModel>.PostAsync(message, url, token);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return Json(new { ID = createdMessage.ID, SenderID = createdMessage.SenderID, ReceiverID = createdMessage.ReceiverID, Text = createdMessage.Text, DateCreated = createdMessage.DateCreated }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetMessages(int senderID, int receiverID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetMessages?senderID={0}&receiverID={1}", senderID, receiverID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var messages = await HttpClientBuilder<MessageModel>.GetListAsync(url, token);
				var viewModels = new List<MessageViewModel>();
				foreach (MessageModel m in messages)
				{
					viewModels.Add(MessageConverter.FromBasicToVisual(m));
				}
				ViewBag.ReceiverID = receiverID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetMessages");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetUnreadMessages(int receiverID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetUnreadMessages?receiverID={0}", receiverID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var messages = await HttpClientBuilder<MessageModel>.GetListAsync(url, token);
				var viewModels = new List<MessageViewModel>();
				foreach (MessageModel m in messages)
				{
					viewModels.Add(MessageConverter.FromBasicToVisual(m));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetUnreadMessages");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> ReadMessages(int senderID, int receiverID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != senderID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/CreateMessage?SenderID={0}&ReceiverID={1}", senderID, receiverID);
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

		public ActionResult GetChatMessages(int senderID, int receiverID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetMessages?senderID={0}&receiverID={1}", senderID, receiverID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var messages = Task.Run<List<MessageModel>>(() => HttpClientBuilder<MessageModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<MessageViewModel>();
				foreach (MessageModel m in messages)
				{
					viewModels.Add(MessageConverter.FromBasicToVisual(m));
				}
				ViewBag.ReceiverID = receiverID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "GetMessages");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> ChatRoom(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				ChatUser.AddOnlineUser(user, "", HttpContext.Request.Cookies["ASP.NET_SessionId"].Value);
				url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
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
				HandleErrorInfo info = new HandleErrorInfo(ex, "Profile", "ChatRoom");
				return View("Error", info);
			}
		}
    }
}