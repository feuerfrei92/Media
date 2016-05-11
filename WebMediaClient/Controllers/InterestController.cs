using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

		public async Task<ActionResult> GetAllInterests()
		{
			try
			{
				string url = "http://localhost:8080/api/Interest/GetAllInterests";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "GetAllInterests");
				return View("Error", info);
			}
		}

		public ActionResult CreateInterest(int userID)
		{
			ViewBag.AuthorID = userID;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateInterest(int userID, InterestViewModel interestModel)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/CreateInterest?AuthorID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = InterestConverter.FromVisualToBasic(interestModel);
				var createdInterest = await HttpClientBuilder<InterestModel>.PostAsync(interest, url, token);
				var viewModel = InterestConverter.FromBasicToVisual(createdInterest);
				ViewBag.AuthorID = userID;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "CreateInterest");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateInterest(int ID, InterestViewModel interestModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				if (((UserModel)HttpContext.Session["currentUser"]).ID != interestModel.AuthorID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Interest/UpdateInterest?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = InterestConverter.FromVisualToBasic(interestModel);
				var updatedInterest = await HttpClientBuilder<InterestModel>.PutAsync(interest, url, token);
				var viewModel = InterestConverter.FromBasicToVisual(updatedInterest);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "UpdateInterest");
				return View("Error", info);
			}
		}

		[HttpDelete]
		public ActionResult DeleteInterest(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Interest/DeleteInterest?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<InterestModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "DeleteInterest");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetInterestByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
                var viewModel = InterestConverter.FromBasicToVisual(interest);
				ViewBag.ID = ID;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "GetInterestByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetInterestByIDRaw(int ID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
				return Json(new { ID = interest.ID, Name = interest.Name, AuthorID = interest.AuthorID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetInterestByName(string name)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByName?Name={0}", name);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
                var viewModel = InterestConverter.FromBasicToVisual(interest);
				ViewBag.Name = name;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
                return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "GetInterestByName");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetInterestByNameRaw(string name)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Interest/GetInterestByName?Name={0}", name);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interest = await HttpClientBuilder<InterestModel>.GetAsync(url, token);
				return Json(new { ID = interest.ID, Name = interest.Name, AuthorID = interest.AuthorID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public async Task<ActionResult> SearchInterestNameByString(string partialName)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/SearchInterestNameByString?PartialName={0}", partialName);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "SearchInterestNameByString");
				return View("Error", info);
			}
		}

		public ActionResult GetInterestsForUser(int userID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Interest/GetInterestsForUser?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var interests = await HttpClientBuilder<InterestModel>.GetListAsync(url, token);
				var interests = Task.Run<List<InterestModel>>(() => HttpClientBuilder<InterestModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<InterestViewModel>();
                foreach (InterestModel i in interests)
                {
                    viewModels.Add(InterestConverter.FromBasicToVisual(i));
                }
				return PartialView(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Interest", "GetInterestsForUser");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetFollower(int userID, int interestID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Interest/GetFollower?UserID={0}&InterestID={1}", userID, interestID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var follower = await HttpClientBuilder<FollowerInfo>.GetAsync(url, token);
				return Json(new { Follower = follower }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}