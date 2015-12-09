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
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllComments(string token)
		{
			try
			{
				string url = "http://localhost:8080/api/Comment/GetAllComments";
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> CreateComment(int topicID, CommentViewModel commentModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

		public async Task<ActionResult> UpdateComment(int ID, int topicID, CommentViewModel commentModel, string token)
		{
			//TODO: Implement constructors or static methods in converters 
			return View();
		}

		public ActionResult DeleteComment(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/DeleteComment?ID={0}", ID);
				HttpClientBuilder<CommentModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetCommentByID(int ID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentByID?ID={0}", ID);
				var comment = await HttpClientBuilder<CommentModel>.GetAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetCommentsByTopicID(int topicID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByTopicID?TopicID={0}", topicID);
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetCommentsByAuthorID(int authorID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByAuthorID?AuthorID={0}", authorID);
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpGet]
		public async Task<ActionResult> SearchByTextContent(string content, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/SearchByTextContent?Content={0}", content);
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchCommentsByCriteria(CommentCriteriaViewModel criteria, string token)
		{
			//TODO: Add all current converters and implement constructors or static methods in converters
			return View();
		}
    }
}