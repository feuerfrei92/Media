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
    public class InterestController : Controller
    {
        // GET: Interest
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllInterests(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Interest/GetAllInterests";
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult CreateInterest(int userID)
		{
			ViewBag.AuthorID = userID;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateInterest(int userID, InterestViewModel interestModel, string token)
		{
            string url = string.Format("http://localhost:8080/api/Interest/CreateInterest?AuthorID={0}", userID);
            var interest = InterestConverter.FromVisualToBasic(interestModel);
            var createdInterest = await HttpClientBuilder<InterestModel>.PostAsync(interest, url, token);
            var viewModel = InterestConverter.FromBasicToVisual(createdInterest);
			ViewBag.AuthorID = userID;
            return View(viewModel);
		}

		public async Task<ActionResult> UpdateInterest(int ID, InterestViewModel interestModel, string token)
		{
            string url = string.Format("http://localhost:8080/api/Interest/UpdateInterest?ID={0}", ID);
            var interest = InterestConverter.FromVisualToBasic(interestModel);
            var updatedInterest = await HttpClientBuilder<InterestModel>.PostAsync(interest, url, token);
            var viewModel = InterestConverter.FromBasicToVisual(updatedInterest);
            return View(viewModel);
		}

		public ActionResult DeleteInterest(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/DeleteInterest?ID={0}", ID);
				HttpClientBuilder<InterestModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetInterestByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByID?ID={0}", ID);
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
                var viewModel = InterestConverter.FromBasicToVisual(interest);
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetInterestByName(string name, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByName?Name={0}", name);
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
                var viewModel = InterestConverter.FromBasicToVisual(interest);
				ViewBag.Name = name;
                return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetInterestByNameRaw(string name, string token)
		{
			string url = string.Format("http://localhost:8080/api/Interest/GetInterestByName?Name={0}", name);
			var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
			return Json(new { ID = interest.ID, Name = interest.Name }, JsonRequestBehavior.AllowGet);
			
		}

		[HttpGet]
		public async Task<ActionResult> SearchInterestNameByString(string partialName, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/SearchInterestNameByString?PartialName={0}", partialName);
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetInterestsForUser(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestsForUser?UserID={0}", userID);
				//var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
				var interests = Task.Run<List<InterestModel>>(() => HttpClientBuilder<InterestModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return PartialView(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetFollower(int userID, int interestID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Interest/GetFollower?UserID={0}&InterestID={1}", userID, interestID);
			var follower = await HttpClientBuilder<FollowerInfo>.GetAsync(url, token);
			if (follower == null)
				return HttpNotFound("No follower");
			else return Json(new { Follower = follower }, JsonRequestBehavior.AllowGet);
		}
    }
}