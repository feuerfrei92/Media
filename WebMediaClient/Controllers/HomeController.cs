using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebMediaClient.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.User = GlobalUser.User;

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
			string token = TempData["token"].ToString();
			string url = "http://localhost:8080/api/User/GetCurrentUser";
			UserModel user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
			ViewBag.Message = string.Format("Welcome, {0}. Redirecting you to home page...", user.Username);
			ViewBag.Token = token;
			GlobalUser.User = user;
			return View();
		}

		[AllowAnonymous]
		public ActionResult LogOff()
		{
			GlobalUser.User = null;
			return View();
		}

		public ActionResult SetUser(UserModel user)
		{
			GlobalUser.User = user;
			return Json(new { ID = user.ID, Username = user.Username }, JsonRequestBehavior.AllowGet);
		}
	}
}