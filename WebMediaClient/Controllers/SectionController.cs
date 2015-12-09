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

		public async Task<ActionResult> UpdateSection(int ID, SectionViewModel sectionModel, string token, int? parentID = null)
		{
            string url = string.Format("http://localhost:8080/api/Section/CreateSection?ID={0}&ParentID={1}", ID, parentID);
            var section = SectionConverter.FromVisualToBasic(sectionModel);
            var updatedSection = await HttpClientBuilder<SectionModel>.PostAsync(section, url, token);
            var viewModel = SectionConverter.FromBasicToVisual(updatedSection);
            return View(viewModel);
		}

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
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetSectionByParentID(int parentID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Section/GetSectionByParentID?ParentID={0}", parentID);
				var section = await HttpClientBuilder<SectionModel>.GetAsync(url, token);
                var viewModel = SectionConverter.FromBasicToVisual(section);
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}