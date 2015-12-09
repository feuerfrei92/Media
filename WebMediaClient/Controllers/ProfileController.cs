using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public async Task<ActionResult> UpdateProfile(int ID, int userID, ProfileViewModel profileModel, string token)
		{
            try
            {
                string url = string.Format("http://localhost:8080/api/Profile/UpdateProfile?ID={0}&UserID={1}", ID, userID);
                var profile = ProfileConverter.FromVisualToBasic(profileModel);
                var updatedProfile = await HttpClientBuilder<ProfileModel>.PostAsync(profile, url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(updatedProfile);
                return View(viewModel);
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
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetProfileByID?ID={0}", ID);
				var profile = await HttpClientBuilder<ProfileModel>.GetAsync(url, token);
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
				return View(viewModel);
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
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
                return View(viewModel);
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
                var viewModel = ProfileConverter.FromBasicToVisual(profile);
                return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

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
            return View();
		}

		public async Task<ActionResult> GetAllFriends(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Profile/GetAllFriends?UserID={0}", userID);
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
    }
}