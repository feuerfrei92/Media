using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
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

		public async Task<ActionResult> GetUserByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/User/GetUserByID?ID={0}", ID);
				var user = await HttpClientBuilder<UserModel>.GetAsync(url, token);
				return View();
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
				return View();
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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}