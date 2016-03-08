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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllUsers(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/User/GetAllUsers";
				var users = await HttpClientBuilder<UserModel>.GetListAsync(url, token);
                var viewModels = new List<UserViewModel>();
                foreach (UserModel u in users)
                {
                    viewModels.Add(UserConverter.FromBasicToVisual(u));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetCurrentUser(string token)
		{
			string url = "http://localhost:8080/api/User/GetCurrentUser";
			var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
			return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult DeleteUser(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/DeleteUser?ID={0}", ID);
				HttpClientBuilder<UserModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetUserByID(int ID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", ID);
				//var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				var user = Task.Run<UserModel>(() => HttpClientBuilder<UserModel>.GetAsync(url, token)).Result;
                var viewModel = UserConverter.FromBasicToVisual(user);
				return View(viewModel);
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetUserByIDRaw(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", ID);
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetUserByUsername(string username, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetUserByUsername?Username={0}", username);
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpGet]
		public async Task<ActionResult> SearchUsernameByString(string partialUsername, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/SearchUsernameByString?PartialUsername={0}", partialUsername);
				var users = await HttpClientBuilder<UserModel>.GetListAsync(url, token);
                var viewModels = new List<UserViewModel>();
                foreach (UserModel u in users)
                {
                    viewModels.Add(UserConverter.FromBasicToVisual(u));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpPost]
		public async Task<ActionResult> CreateVisit(int userID, int topicID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/CreateVisit?UserID={0}&TopicID={1}", userID, topicID);
				var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateVisit(int userID, int topicID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/UpdateVisit?UserID={0}&TopicID={1}", userID, topicID);
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpGet]
		public async Task<ActionResult> GetVisitsByUserID(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetVisitsByUserID?UserID={0}", userID);
				var visits = await HttpClientBuilder<HttpResponseMessage>.GetListAsync(url, token);
				return Json(new { Visits = visits }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpGet]
		public async Task<ActionResult> GetVisitsByTopicID(int topicID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetVisitsByTopicID?TopicID={0}", topicID);
				var visits = await HttpClientBuilder<HttpResponseMessage>.GetListAsync(url, token);
				return Json(new { Visits = visits }, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetMembershipsForSection(int sectionID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/User/GetMembershipsForSection?SectionID={0}", sectionID);
				//var users = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var users = Task.Run<List<UserModel>>(() => HttpClientBuilder<UserModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<UserViewModel>();
				foreach (UserModel u in users)
				{
					viewModels.Add(UserConverter.FromBasicToVisual(u));
				}
				return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetMembershipsForSectionRaw(int sectionID, int getSpecial, string token)
		{
			string url = string.Format("http://localhost:8080/api/User/GetMembershipsForSectionRaw?SectionID={0}&GetSpecial={1}", sectionID, getSpecial);
			var users = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
			return Json(users, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetPendingMembershipsForSection(int sectionID, string token)
		{
			//try
			//{
			string url = string.Format("http://localhost:8080/api/User/GetPendingMembershipsForSection?SectionID={0}", sectionID);
			//var users = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
			var users = Task.Run<List<UserModel>>(() => HttpClientBuilder<UserModel>.GetListAsync(url, token)).Result;
			var viewModels = new List<UserViewModel>();
			foreach (UserModel u in users)
			{
				viewModels.Add(UserConverter.FromBasicToVisual(u));
			}
			ViewBag.SectionID = sectionID;
			return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}
    }
}