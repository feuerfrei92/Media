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
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult CreateProfile(int userID)
		{
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateProfile(int userID, ProfileViewModel profileModel, string token)
		{
			string url = string.Format("http://localhost:8080/api/Profile/CreateProfile?UserID={0}", userID);
			var profile = ProfileConverter.FromVisualToBasic(profileModel);
			var createdProfile = await HttpClientBuilder<ProfileModel>.PostAsync(profile, url, token);
			var viewModel = ProfileConverter.FromBasicToVisual(createdProfile);
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> AddFriend(int userID, int friendID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Profile/AddFriend?UserID={0}&FriendID={1}", userID, friendID);
			var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPut]
		public async Task<ActionResult> AcceptFriendship(int userID, int friendID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Profile/AcceptFriendship?UserID={0}&FriendID={1}", userID, friendID);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
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
            catch
            {
                return RedirectToAction("Error", "Account");
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
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetProfileByID(int ID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.ID = ID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetProfileByUserID(int userID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.UserID = userID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
                return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetProfileByUsernameRaw(string username, string token)
		{
			string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
			var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
			return Json(new { ID = profile.ID, Username = profile.Username, Name = profile.Name, Age = profile.Age, Gender = profile.Gender }, JsonRequestBehavior.AllowGet);
		}

		public async Task<ActionResult> GetProfileByUsername(string username, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				ViewBag.Username = username;
                return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult SearchProfilesByCriteria(string token)
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SearchProfilesByCriteria(ProfileCriteriaViewModel criteria, string token)
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

		public ActionResult DisplayProfileSearchResults()
		{
			List<ProfileViewModel> viewModels = new List<ProfileViewModel>();
			if (TempData["viewModels"] != null)
				viewModels = (List<ProfileViewModel>)TempData["viewModels"];
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
			return View(viewModels);
		}

		public ActionResult GetAllFriends(int userID, string token)
		{
			////try
			////{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
				//var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				var profiles = Task.Run<List<ProfileModel>>(() => HttpClientBuilder<ProfileModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<ProfileViewModel>();
                foreach (ProfileModel p in profiles)
                {
                    viewModels.Add(ProfileConverter.FromBasicToVisual(p));
                }
				return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetFriendship(int userID, int friendID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Profile/GetFriendship?UserID={0}&FriendID={1}", userID, friendID);
			var friendship = await HttpClientBuilder<FriendshipInfo>.GetAsync(url, token);
			if (friendship == null)
				return HttpNotFound("No friendship");
			else return Json(new { Friendship = friendship }, JsonRequestBehavior.AllowGet);
		}
    }
}