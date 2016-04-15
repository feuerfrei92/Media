using PagedList;
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
    public class TopicController : Controller
    {
        // GET: Topic
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllTopics()
		{
			try
			{
				string url = "http://localhost:8080/api/Topic/GetAllTopics";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
				return View(viewModels);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetAllTopics");
				return View("Error", info);
			}
		}

		public ActionResult CreateTopic(int sectionID, int authorID)
		{
			if (Request.UrlReferrer == null)
				return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

			ViewBag.SectionID = sectionID;
			ViewBag.AuthorID = authorID;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateTopic(int sectionID, int authorID, TopicViewModel topicModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/CreateTopic?SectionID={0}&AuthorID={1}", sectionID, authorID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topic = TopicConverter.FromVisualToBasic(topicModel);
				var createdTopic = await HttpClientBuilder<TopicModel>.PostAsync(topic, url, token);
				var viewModel = TopicConverter.FromBasicToVisual(createdTopic);
				return RedirectToAction("GetTopicByID", "Topic", new { ID = viewModel.ID });
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "CreateTopic");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateTopic(int ID, int sectionID, TopicViewModel topicModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/CreateTopic?ID={0}&SectionID={1}", ID, sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topic = TopicConverter.FromVisualToBasic(topicModel);
				var updatedTopic = await HttpClientBuilder<TopicModel>.PostAsync(topic, url, token);
				var viewModel = TopicConverter.FromBasicToVisual(updatedTopic);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "UpdateTopic");
				return View("Error", info);
			}
		}

		[HttpDelete]
		public ActionResult DeleteTopic(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/DeleteTopic?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				HttpClientBuilder<TopicModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "DeleteTopic");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetTopicByID(int ID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
                var viewModel = TopicConverter.FromBasicToVisual(topic);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.Page = page;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetTopicByID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetTopicByIDRaw(int ID)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
				return Json(new { SectionID = topic.SectionID, TopicType = topic.TopicType }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public async Task<ActionResult> GetTopicByOwnerAndType(int ownerID, string topicType, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByOwnerAndType?OwnerID={0}&TopicType={1}", ownerID, topicType);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
				var viewModel = TopicConverter.FromBasicToVisual(topic);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.Page = page;
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetTopicByID");
				return View("Error", info);
			}
		}

		public ActionResult GetTopicsBySectionID(int sectionID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicsBySectionID?SectionID={0}", sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
				ViewBag.SectionID = sectionID;

				if (page == null)
					return View(viewModels.ToPagedList(1, 20));
				else
					return View(viewModels.ToPagedList(page.Value, 20));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetTopicsBySectionID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetTopicsByAuthorIDAndSectionID(int authorID, int sectionID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicsByAuthorIDAndSectionID?AuthorID={0}&SectionID={1}", authorID, sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				ViewBag.AuthorID = authorID;
				ViewBag.SectionID = sectionID;

				if (page == null)
					return View(viewModels.ToPagedList(1, 20));
				else
					return View(viewModels.ToPagedList(page.Value, 20));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetTopicsBySectionIDAndAuthorID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetTopicsByAuthorID(int authorID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByAuthorID?AuthorID={0}", authorID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
				ViewBag.AuthorID = authorID;

				if (page == null)
					return View(viewModels.ToPagedList(1, 20));
				else
					return View(viewModels.ToPagedList(page.Value, 20));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "GetTopicsByAuthorID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> SearchByTopicName(string name, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/SearchByTopicName?Name={0}", name);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}

				if (page == null)
					return View(viewModels.ToPagedList(1, 20));
				else
					return View(viewModels.ToPagedList(page.Value, 20));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "SearchByTopicName");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> SearchTopicsByCriteria(TopicCriteriaViewModel criteria)
		{
			try
			{
				string url = "http://localhost:8080/api/Topic/SearchTopicsByCriteria";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var topicCriteria = TopicConverter.CriteriaFromVisualToBasic(criteria);
				var topics = await HttpClientBuilder<TopicCriteria>.GetListAsync<TopicModel>(topicCriteria, url, token);
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Topic", "SearchTopicsByCriteria");
				return View("Error", info);
			}
		}
    }
}