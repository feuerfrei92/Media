using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebMediaClient.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
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
		public async Task<ActionResult> SuccessfulLogin(string token)
		{
			string url = "http://localhost:8080/api/Comment/GetAllComments";
			string username = await HttpClientBuilder<string>.GetAsync(url, token);
			ViewBag.Message = string.Format("Welcome, {0}. Redirecting you to home page...", username);
			ViewBag.Token = token;
			return View();
		}
	}
}