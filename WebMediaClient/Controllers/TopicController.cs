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
    public class TopicController : Controller
    {
        // GET: Topic
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllTopics(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Topic/GetAllTopics";
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult CreateTopic(int sectionID, int authorID)
		{
			ViewBag.SectionID = sectionID;
			ViewBag.AuthorID = authorID;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> CreateTopic(int sectionID, int authorID, TopicViewModel topicModel, string token)
		{
            string url = string.Format("http://localhost:8080/api/Topic/CreateTopic?SectionID={0}&AuthorID={1}", sectionID, authorID);
            var topic = TopicConverter.FromVisualToBasic(topicModel);
            var createdTopic = await HttpClientBuilder<TopicModel>.PostAsync(topic, url, token);
            var viewModel = TopicConverter.FromBasicToVisual(createdTopic);
			return RedirectToAction("GetTopicByID", "Topic", new { ID = viewModel.ID });
		}

		[HttpPut]
		public async Task<ActionResult> UpdateTopic(int ID, int sectionID, TopicViewModel topicModel, string token)
		{
            string url = string.Format("http://localhost:8080/api/Topic/CreateTopic?ID={0}&SectionID={1}", ID, sectionID);
            var topic = TopicConverter.FromVisualToBasic(topicModel);
            var updatedTopic = await HttpClientBuilder<TopicModel>.PostAsync(topic, url, token);
            var viewModel = TopicConverter.FromBasicToVisual(updatedTopic);
            return View(viewModel);
		}

		[HttpDelete]
		public ActionResult DeleteTopic(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/DeleteTopic?ID={0}", ID);
				HttpClientBuilder<TopicModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetTopicByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByID?ID={0}", ID);
				var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
                var viewModel = TopicConverter.FromBasicToVisual(topic);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetTopicsBySectionID(int sectionID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicsBySectionID?SectionID={0}", sectionID);
				//var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var topics = Task.Run<List<TopicModel>>(() => HttpClientBuilder<TopicModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
                return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetTopicForProfile(int profileID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicForProfile?ProfileID={0}", profileID);
				//var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
				var topic = Task.Run<TopicModel>(() => HttpClientBuilder<TopicModel>.GetAsync(url, token)).Result;
				var viewModel = TopicConverter.FromBasicToVisual(topic);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public ActionResult GetTopicForInterest(int interestID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicForInterest?InterestID={0}", interestID);
				//var topic = await HttpClientBuilder<TopicModel>.GetAsync(url, token);
				var topic = Task.Run<TopicModel>(() => HttpClientBuilder<TopicModel>.GetAsync(url, token)).Result;
				var viewModel = TopicConverter.FromBasicToVisual(topic);
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				return View(viewModel);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetTopicsByAuthorID(int authorID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByAuthorID?AuthorID={0}", authorID);
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
                var viewModels = new List<TopicViewModel>();
                foreach (TopicModel t in topics)
                {
                    viewModels.Add(TopicConverter.FromBasicToVisual(t));
                }
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchByTopicName(string name, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/SearchByTopicName?Name={0}", name);
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				var viewModels = new List<TopicViewModel>();
				foreach (TopicModel t in topics)
				{
					viewModels.Add(TopicConverter.FromBasicToVisual(t));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchTopicsByCriteria(TopicCriteriaViewModel criteria, string token)
		{
            string url = "http://localhost:8080/api/Topic/SearchTopicsByCriteria";
            var topicCriteria = TopicConverter.CriteriaFromVisualToBasic(criteria);
            var topics = await HttpClientBuilder<TopicCriteria>.GetListAsync<TopicModel>(topicCriteria, url, token);
            var viewModels = new List<TopicViewModel>();
            foreach (TopicModel t in topics)
            {
                viewModels.Add(TopicConverter.FromBasicToVisual(t));
            }
            return View();
		}
    }
}