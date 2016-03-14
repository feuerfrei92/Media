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

		public async Task<ActionResult> GetAllProfiles(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Profile/GetAllProfiles";
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
				return View("Error");
			}
		}

		public ActionResult CreateProfile(int userID)
		{
			if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateProfile(int userID, ProfileViewModel profileModel, string token)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/CreateProfile?UserID={0}", userID);
				var profile = ProfileConverter.FromVisualToBasic(profileModel);
				var createdProfile = await HttpClientBuilder<ProfileModel>.PostAsync(profile, url, token);
				var viewModel = ProfileConverter.FromBasicToVisual(createdProfile);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		[HttpPost]
		public async Task<ActionResult> AddFriend(int userID, int friendID, string token)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/AddFriend?UserID={0}&FriendID={1}", userID, friendID);
				var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPut]
		public async Task<ActionResult> AcceptFriendship(int userID, int friendID, string token)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/AcceptFriendship?UserID={0}&FriendID={1}", userID, friendID);
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
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateProfile(int ID, ProfileViewModel profileModel, string token)
		{
            try
            {
                string url = string.Format("http://localhost:8080/api/Profile/UpdateProfile?ID={0}", ID);
                var profile = ProfileConverter.FromVisualToBasic(profileModel);
                var updatedProfile = await HttpClientBuilder<ProfileModel>.PutAsync(profile, url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(updatedProfile);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return RedirectToAction("Index", "Home");
            }
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public ActionResult DeleteProfile(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/DeleteProfile?ID={0}", ID);
				HttpClientBuilder<ProfileModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public async Task<ActionResult> GetProfileByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.ID = ID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public async Task<ActionResult> GetProfileByIDRaw(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetProfileByUserID(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.UserID = userID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
                return View(viewModel);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public async Task<ActionResult> GetProfileByUsernameRaw(string username, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetProfileByUsername(string username, string token)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.Username = username;
                return View(viewModel);
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public ActionResult SearchProfilesByCriteria(string token)
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SearchProfilesByCriteria(ProfileCriteriaViewModel criteria, string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Profile/SearchProfilesByCriteria";
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
				return View("Error");
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
				return View("Error");
			}
		}

		public ActionResult GetAllFriends(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
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
				return View("Error");
			}
		}

		public async Task<ActionResult> GetCommonFriends(int userID, int targetID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetCommonFriends?UserID={0}&TargetID={1}", userID, targetID);
				var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				return Json(new { Count = profiles.Count }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetFriendship(int userID, int friendID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetFriendship?UserID={0}&FriendID={1}", userID, friendID);
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
		public async Task<ActionResult> CreateMessage(int senderID, int receiverID, MessageViewModel messageModel, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/CreateMessage?SenderID={0}&ReceiverID={1}", senderID, receiverID);
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

		public async Task<ActionResult> GetMessages(int senderID, int receiverID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetMessages?senderID={0}&receiverID={1}", senderID, receiverID);
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
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}