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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateInterest(int userID, InterestViewModel interestModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

		public async Task<ActionResult> UpdateInterest(int ID, InterestViewModel interestModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
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
				return View();
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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpGet]
		public async Task<ActionResult> SearchInterestNameByString(string partialName, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/SearchInterestNameByString?PartialName={0}", partialName);
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetInterestsForUser(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestsForUser?UserID={0}", userID);
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}