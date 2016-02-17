using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public async Task<ActionResult> GetAllSections(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Section/GetAllSections";
				var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
                var viewModels = new List<SectionViewModel>();
                foreach (SectionModel s in sections)
                {
                    viewModels.Add(SectionConverter.FromBasicToVisual(s));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateSection(SectionViewModel sectionModel, string token, int? parentID = null)
		{
            string url = string.Format("http://localhost:8080/api/Section/CreateSection?ParentID={0}", parentID);
            var section = SectionConverter.FromVisualToBasic(sectionModel);
            var createdSection = await HttpClientBuilder<SectionModel>.PutAsync(section, url, token);
            var viewModel = SectionConverter.FromBasicToVisual(createdSection);
            return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> UpdateSection(int ID, SectionViewModel sectionModel, string token, int? parentID = null)
		{
            string url = string.Format("http://localhost:8080/api/Section/CreateSection?ID={0}&ParentID={1}", ID, parentID);
            var section = SectionConverter.FromVisualToBasic(sectionModel);
            var updatedSection = await HttpClientBuilder<SectionModel>.PostAsync(section, url, token);
            var viewModel = SectionConverter.FromBasicToVisual(updatedSection);
            return View(viewModel);
		}

		[HttpDelete]
		public ActionResult DeleteSection(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/DeleteSection?ID={0}", ID);
				HttpClientBuilder<SectionModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetSectionByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetSectionByID?ID={0}", ID);
				var section = await HttpClientBuilder<SectionModel>.GetAsync(url, token);
                var viewModel = SectionConverter.FromBasicToVisual(section);
				ViewBag.User = GlobalUser.User;
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetSectionsByParentID(int parentID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetSectionsByParentID?ParentID={0}", parentID);
				//var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var sections = Task.Run<List<SectionModel>>(() => HttpClientBuilder<SectionModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<SectionViewModel>();
				foreach(SectionModel s in sections)
				{
					viewModels.Add(SectionConverter.FromBasicToVisual(s));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchBySectionName(string name, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/SearchBySectionName?Name={0}", name);
				var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var viewModels = new List<SectionViewModel>();
				foreach (SectionModel s in sections)
				{
					viewModels.Add(SectionConverter.FromBasicToVisual(s));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpPost]
		public async Task<ActionResult> AddMembership(int sectionID, int userID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Section/AddMembership?SectionID={0}&UserID={1}", sectionID, userID);
			var response = await HttpClientBuilder<HttpResponseMessage>.PostEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPut]
		public async Task<ActionResult> UpdateMembership(int sectionID, int userID, string role, DateTime? suspension, string token)
		{
			string url = string.Format("http://localhost:8080/api/Section/AddMembership?SectionID={0}&UserID={1}&Role={2}&Suspension={3}", sectionID, userID, role, suspension);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteMembership(int sectionID, int userID, string token)
		{
			string url = string.Format("http://localhost:8080/api/Section/DeleteMembership?SectionID={0}&UserID={1}", sectionID, userID);
			HttpClientBuilder<HttpResponseMessage>.DeleteAsync(url, token);
			return View();
		}

		public ActionResult GetMembershipsForUser(int userID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetMembershipsForUser?UserID={0}", userID);
				//var sections = await HttpClientBuilder<SectionModel>.GetListAsync(url, token);
				var sections = Task.Run<List<SectionModel>>(() => HttpClientBuilder<SectionModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<SectionViewModel>();
                foreach (SectionModel s in sections)
                {
                    viewModels.Add(SectionConverter.FromBasicToVisual(s));
                }
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}