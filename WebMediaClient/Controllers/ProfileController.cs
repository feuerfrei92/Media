using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> UpdateProfile(int ID, int userID, ProfileViewModel profileModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
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
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetProfileByUserID(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUserID?UserID={0}", userID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetProfileByUsername(string username, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByUsername?Username={0}", username);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchProfilesByCriteria(ProfileCriteriaViewModel criteria, string token)
		{
			//TODO: Add all current converters and implement constructors or static methods in converters
			return View();
		}

		public async Task<ActionResult> GetAllFriends(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
				var profiles = await HttpClientBuilder<ProfileModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}