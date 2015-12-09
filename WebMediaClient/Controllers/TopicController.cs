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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateTopic(int sectionID, TopicViewModel topicModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

		public async Task<ActionResult> UpdateTopic(int ID, int sectionID, TopicViewModel topicModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

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
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetTopicsBySectionID(int sectionID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicsByUserID?SectionID={0}", sectionID);
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetTopicsByAuthorID(int authorID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Topic/GetTopicByAuthorID?AuthorID={0}", authorID);
				var topics = await HttpClientBuilder<TopicModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchTopicsByCriteria(TopicCriteriaViewModel criteria, string token)
		{
			//TODO: Add all current converters and implement constructors or static methods in converters
			return View();
		}
    }
}