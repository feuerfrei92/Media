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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateSection(SectionViewModel profileModel, string token, int? parentID = null)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

		public async Task<ActionResult> UpdateSection(int ID, SectionViewModel profileModel, string token, int? parentID = null)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
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
				return View();
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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}
    }
}