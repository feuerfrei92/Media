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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllUsers()
		{
			try
			{
				string url = "http://localhost:8080/api/User/GetAllUsers";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var users = await HttpClientBuilder<UserModel>.GetListAsync(url, token);
                var viewModels = new List<UserViewModel>();
                foreach (UserModel u in users)
                {
                    viewModels.Add(UserConverter.FromBasicToVisual(u));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "GetAllUsers");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetCurrentUser()
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = "http://localhost:8080/api/User/GetCurrentUser";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return Json(new { ID = user.ID, Username = user.Username, IsAdmin = user.IsAdmin }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteUser(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/DeleteUser?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<UserModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "DeleteUser");
				return View("Error", info);
			}
		}

		public ActionResult GetAuthorInfo(int userID, bool anonymous = false)
		{
			try
			{
				if (!anonymous)
				{
					string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
					string token = "";
					if (HttpContext.Session["token"] != null)
						token = HttpContext.Session["token"].ToString();
					else
						token = null;
					//var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
					var profile = Task.Run<ProfileModel>(() => HttpClientBuilder<ProfileModel>.GetAsync(url, token)).Result;
					var viewModel = ProfileConverter.FromBasicToVisual(profile);
					return View(viewModel);
				}
				else
				{
					string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", userID);
					string token = "";
					if (HttpContext.Session["token"] != null)
						token = HttpContext.Session["token"].ToString();
					else
						token = null;
					//var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
					var user = Task.Run<UserModel>(() => HttpClientBuilder<UserModel>.GetAsync(url, token)).Result;
					var viewModel = UserConverter.FromBasicToVisual(user);
					return View("GetUserByID", viewModel);
				}
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "GetUserByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetUserByIDRaw(int ID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetUserByUsername(string username)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetUserByUsername?Username={0}", username);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public async Task<ActionResult> SearchUsernameByString(string partialUsername)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/SearchUsernameByString?PartialUsername={0}", partialUsername);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var users = await HttpClientBuilder<UserModel>.GetListAsync(url, token);
                var viewModels = new List<UserViewModel>();
                foreach (UserModel u in users)
                {
                    viewModels.Add(UserConverter.FromBasicToVisual(u));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "SearchUsernameByString");
				return View("Error", info);
			}
		}

		[HttpPost]
		public async Task<ActionResult> CreateVisit(int userID, int topicID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/CreateVisit?UserID={0}&TopicID={1}", userID, topicID);
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
		public async Task<ActionResult> UpdateVisit(int userID, int topicID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/UpdateVisit?UserID={0}&TopicID={1}", userID, topicID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				//var response = Task.Run<HttpResponseMessage>(() => HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token)).Result;
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public async Task<ActionResult> GetVisitsByUserID(int userID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetVisitsByUserID?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var visits = await HttpClientBuilder<HttpResponseMessage>.GetListAsync(url, token);
				return Json(new { Visits = visits }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public async Task<ActionResult> GetVisitsByTopicID(int topicID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetVisitsByTopicID?TopicID={0}", topicID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var visits = await HttpClientBuilder<HttpResponseMessage>.GetListAsync(url, token);
				return Json(new { Visits = visits }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetMembershipsForSection(int sectionID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetMembershipsForSection?SectionID={0}", sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var users = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var users = Task.Run<List<UserModel>>(() => HttpClientBuilder<UserModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<UserViewModel>();
				foreach (UserModel u in users)
				{
					viewModels.Add(UserConverter.FromBasicToVisual(u));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "GetMembershipsForSection");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetMembershipsForSectionRaw(int sectionID, int getSpecial)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetMembershipsForSectionRaw?SectionID={0}&GetSpecial={1}", sectionID, getSpecial);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var users = await HttpClientBuilder<UserModel>.GetListAsync(url, token);
				return Json(users, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetPendingMembershipsForSection(int sectionID)
		{
			try
			{
				//if(!ControllerContext.IsChildAction)
				//	return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/GetPendingMembershipsForSection?SectionID={0}", sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var users = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var users = Task.Run<List<UserModel>>(() => HttpClientBuilder<UserModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<UserViewModel>();
				foreach (UserModel u in users)
				{
					viewModels.Add(UserConverter.FromBasicToVisual(u));
				}
				ViewBag.SectionID = sectionID;
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "User", "GetPendingMembershipsForSection");
				return View("Error", info);
			}
		}

		[HttpPost]
		public async Task<ActionResult> AddUserToGroup(int userID, int groupID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/AddUserToGroup?UserID={0}&GroupID={1}", userID, groupID);
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

		[HttpDelete]
		public ActionResult RemoveUserFromGroup(int userID, int groupID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/User/RemoveUserFromGroup?UserID={0}&GroupID={1}", userID, groupID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<HttpResponseMessage>.DeleteAsync(url, token);
				return Json(new { Response = System.Net.HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetGroupsForUser(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetGroupsForUser?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var groups = Task.Run<List<DiscussionModel>>(() => HttpClientBuilder<DiscussionModel>.GetListAsync(url, token)).Result;
				return Json(new { Groups = groups }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}