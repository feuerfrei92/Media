using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.User = (UserModel)HttpContext.Session["currentUser"];

			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[AllowAnonymous]
		public async Task<ActionResult> SuccessfulLogin()
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string token = HttpContext.Session["token"].ToString();
				string url = "http://localhost:8080/api/User/GetCurrentUser";
				UserModel user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				if (user != null)
				{
					ViewBag.Message = string.Format("Welcome, {0}. Redirecting you to home page...", user.Username);
					HttpContext.Session.Add("currentUser", user);
					if (HttpRuntime.Cache["LoggedInUsers"] != null)
					{
						List<ChatUser> loggedInUsers = (List<ChatUser>)HttpRuntime.Cache["LoggedInUsers"];
						var chatUser = new ChatUser
						{
							ID = user.ID,
							Username = user.Username,
						};
						HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
					}
					ViewBag.UserID = user.ID;
				}
				else
				{
					ViewBag.Message = "Unvalidated user _|_";
					ViewBag.UserID = -1;
					HttpContext.Session.Remove("token");
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Home", "SuccessfulLogin");
				return View("Error", info);
			}
		}

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			try
			{
				UserModel user = (UserModel)HttpContext.Session["currentUser"];
				HttpContext.Session.Remove("currentUser");
				HttpContext.Session.Remove("token");
				if (HttpRuntime.Cache["LoggedInUsers"] != null)
				{
					List<ChatUser> loggedInUsers = (List<ChatUser>)HttpRuntime.Cache["LoggedInUsers"];
					var chatUser = loggedInUsers.Where(c => c.ID == user.ID).FirstOrDefault();
					if (chatUser != null)
					{
						loggedInUsers.Remove(chatUser);
						HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
					}
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Home", "LogOff");
				return View("Error", info);
			}
		}

		public ActionResult SetUser(UserModel user, string token)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				HttpContext.Session.Add("currentUser", user);
				HttpContext.Session.Add("token", token);
				return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Home", "SetUser");
				return View("Error", info);
			}
		}
	}
}