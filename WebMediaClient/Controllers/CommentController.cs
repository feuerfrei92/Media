using RTE;
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
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public async Task<ActionResult> CreateComment(int topicID, int authorID, CommentViewModel commentModel, string token)
		{
			string url = string.Format("http://localhost:8080/api/Comment/CreateComment?TopicID={0}&AuthorID={1}", topicID, authorID);
			var comment = CommentConverter.FromVisualToBasic(commentModel);
			var createdComment = await HttpClientBuilder<CommentModel>.PostAsync(comment, url, token);
			var viewModel = CommentConverter.FromBasicToVisual(createdComment);
			return View(viewModel);
		}

		[HttpPut]
		public async Task<ActionResult> UpdateComment(int ID, int topicID, CommentViewModel commentModel, string token)
		{
            string url = string.Format("http://localhost:8080/api/Comment/CreateComment?ID={0}&TopicID={1}", ID, topicID);
            var comment = CommentConverter.FromVisualToBasic(commentModel);
            var updatedComment = await HttpClientBuilder<CommentModel>.PutAsync(comment, url, token);
            var viewModel = CommentConverter.FromBasicToVisual(updatedComment);
            return View(viewModel);
		}

		[HttpDelete]
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
                var viewModel = CommentConverter.FromBasicToVisual(comment);
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public ActionResult GetCommentsByTopicID(int topicID, string token)
		{
			//try
			//{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByTopicID?TopicID={0}", topicID);
				//var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var comments = Task.Run<List<CommentModel>>(() => HttpClientBuilder<CommentModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.TopicID = topicID;
				return View(viewModels);
			//}
			//catch
			//{
			//	return RedirectToAction("Error", "Account");
			//}
		}

		public async Task<ActionResult> GetCommentsByAuthorID(int authorID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByAuthorID?AuthorID={0}", authorID);
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
				return View(viewModels);
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> GetCommentsByAuthorIDAndSectionID(int authorID, int sectionID, string token)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByAuthorIDAndSectionID?AuthorID={0}&SectionID={1}", authorID, sectionID);
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				return View(viewModels);
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
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
				return View();
			}
			catch
			{
				return RedirectToAction("Error", "Account");
			}
		}

		public async Task<ActionResult> SearchCommentsByCriteria(CommentCriteriaViewModel criteria, string token)
		{
            string url = "http://localhost:8080/api/Comment/SearchCommentsByCriteria";
            var commentCriteria = CommentConverter.CriteriaFromVisualToBasic(criteria);
            var comments = await HttpClientBuilder<CommentCriteria>.GetListAsync<CommentModel>(commentCriteria, url, token);
            var viewModels = new List<CommentViewModel>();
            foreach (CommentModel c in comments)
            {
                viewModels.Add(CommentConverter.FromBasicToVisual(c));
            }
            return View();
		}

		public async Task<ActionResult> UpdateRating(int commentID, bool like, string token)
		{
			string url = string.Format("http://localhost:8080/api/Comment/UpdateRating?CommentID={0}&Like={1}", commentID, like);
			var response = await HttpClientBuilder<HttpResponseMessage>.PutEmptyAsync(url, token);
			return Json(new { Response = response.StatusCode == System.Net.HttpStatusCode.OK ? "OK" : "Error" }, JsonRequestBehavior.AllowGet);
		}
    }
}