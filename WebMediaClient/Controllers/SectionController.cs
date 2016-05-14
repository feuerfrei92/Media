using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMediaClient.Converters;
using WebMediaClient.Models;

namespace WebMediaClient.Controllers
{
    public class SectionController : Controller
    {
        // GET: Section
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllSections()
		{
			try
			{
				string url = "http://localhost:8080/api/Section/GetAllSections";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
                var viewModels = new List<SectionViewModel>();
                foreach (SectionModel s in sections)
                {
                    viewModels.Add(SectionConverter.FromBasicToVisual(s));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "GetAllSections");
				return View("Error", info);
			}
		}

		[HttpPost]
		public async Task<ActionResult> CreateSection(SectionViewModel sectionModel, int authorID, int? parentID = null)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != authorID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/CreateSection?ParentID={0}", parentID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var section = SectionConverter.FromVisualToBasic(sectionModel);
				var createdSection = await HttpClientBuilder<SectionModel>.PostAsync(section, url, token);
				var viewModel = SectionConverter.FromBasicToVisual(createdSection);
				ViewBag.ParentID = parentID;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "CreateSection");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateSection(int ID, SectionViewModel sectionModel, int? parentID = null)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/CreateSection?ID={0}&ParentID={1}", ID, parentID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var section = SectionConverter.FromVisualToBasic(sectionModel);
				var updatedSection = await HttpClientBuilder<SectionModel>.PutAsync(section, url, token);
				var viewModel = SectionConverter.FromBasicToVisual(updatedSection);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "UpdateSection");
				return View("Error", info);
			}
		}

		[HttpDelete]
		public ActionResult DeleteSection(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/DeleteSection?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<SectionModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "DeleteSection");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetSectionByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetSectionByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var section = await HttpClientBuilder<SectionModel>.GetAsync(url, token);
                var viewModel = SectionConverter.FromBasicToVisual(section);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "GetSectionByID");
				return View("Error", info);
			}
		}

		public ActionResult GetSectionsByParentID(int parentID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetSectionsByParentID?ParentID={0}", parentID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var sections = Task.Run<List<SectionModel>>(() => HttpClientBuilder<SectionModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<SectionViewModel>();
				foreach(SectionModel s in sections)
				{
					viewModels.Add(SectionConverter.FromBasicToVisual(s));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "GetSectionsByParentID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetRoot(int sectionID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/GetRoot?SectionID={0}", sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var section = await HttpClientBuilder<SectionModel>.GetAsync(url, token);
				return Json(new { ID = section.ID, Name = section.Name, ParentID = section.ParentID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> SearchBySectionName(string name)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/SearchBySectionName?Name={0}", name);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var viewModels = new List<SectionViewModel>();
				foreach (SectionModel s in sections)
				{
					viewModels.Add(SectionConverter.FromBasicToVisual(s));
				}
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "SearchBySectionName");
				return View("Error", info);
			}
		}

		[HttpPost]
		public async Task<ActionResult> AddMembership(int sectionID, int userID, bool isAnonymous)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/AddMembership?SectionID={0}&UserID={1}&IsAnonymous={2}", sectionID, userID, isAnonymous);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPut]
		public async Task<ActionResult> AcceptMembership(int sectionID, int userID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/AcceptMembership?SectionID={0}&UserID={1}", sectionID, userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPut]
		public async Task<ActionResult> ChangeVisibilityOfMembership(int sectionID, int userID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/ChangeVisibilityOfMembership?SectionID={0}&UserID={1}", sectionID, userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateMembership(int sectionID, int userID, string role, DateTime? suspension)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/AddMembership?SectionID={0}&UserID={1}&Role={2}&Suspension={3}", sectionID, userID, role, suspension);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
				return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpDelete]
		public ActionResult DeleteMembership(int sectionID, int userID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/DeleteMembership?SectionID={0}&UserID={1}", sectionID, userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<HttpResponseMessage>.DeleteAsync(url, token);
				return View();
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		public ActionResult GetMembershipsForUser(int userID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/GetMembershipsForUser?UserID={0}", userID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var sections = Task.Run<List<SectionModel>>(() => HttpClientBuilder<SectionModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<SectionViewModel>();
                foreach (SectionModel s in sections)
                {
                    viewModels.Add(SectionConverter.FromBasicToVisual(s));
                }
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Section", "GetMembershipsForUser");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetMembership(int userID, int sectionID)
		{
			try
			{
				if (((UserModel)HttpContext.Session["currentUser"]).ID != userID)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Section/GetMembership?UserID={0}&SectionID={1}", userID, sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var membership = await HttpClientBuilder<MembershipInfo>.GetAsync(url, token);
				return Json(new { ID = membership.ID, SectionID = membership.SectionID, UserID = membership.UserID, Role = membership.Role.ToString(), SuspendedUntil = membership.SuspendedUntil, IsAccepted = membership.IsAccepted, Anonymous = membership.Anonymous }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}