using PagedList;
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
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

		public async Task<ActionResult> GetAllComments()
		{
			try
			{
				string url = "http://localhost:8080/api/Comment/GetAllComments";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				return View(viewModels.ToPagedList(1, 15));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetAllComments");
				return View("Error", info);
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public async Task<ActionResult> CreateComment(int topicID, int authorID, CommentViewModel commentModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Comment/CreateComment?TopicID={0}&AuthorID={1}", topicID, authorID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comment = CommentConverter.FromVisualToBasic(commentModel);
				var createdComment = await HttpClientBuilder<CommentModel>.PostAsync(comment, url, token);
				var viewModel = CommentConverter.FromBasicToVisual(createdComment);
				return View(viewModel);
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "CreateComment");
				return View("Error", info);
			}
		}

		public ActionResult UpdateComment(int ID, int topicID)
		{
			ViewBag.ID = ID;
			ViewBag.TopicID = topicID;
			return View();
		}

		[HttpPut]
		public async Task<ActionResult> UpdateComment(int ID, int topicID, CommentViewModel commentModel)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Comment/CreateComment?ID={0}&TopicID={1}", ID, topicID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comment = CommentConverter.FromVisualToBasic(commentModel);
				var updatedComment = await HttpClientBuilder<CommentModel>.PutAsync(comment, url, token);
				var viewModel = CommentConverter.FromBasicToVisual(updatedComment);
				return Redirect(Request.UrlReferrer.ToString());
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "UpdateComment");
				return View("Error", info);
			}
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteComment(int ID)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Comment/DeleteComment?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				await HttpClientBuilder<CommentModel>.DeleteAsync(url, token);
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "DeleteComment");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetCommentByID(int ID)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentByID?ID={0}", ID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comment = await HttpClientBuilder<CommentModel>.GetAsync(url, token);
				return Json(new { ID = comment.ID, Name = comment.Name, Text = comment.Text, DateCreated = comment.DateCreated, AuthorID = comment.AuthorID, TopicID = comment.TopicID }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new { Status = "error", Message = "An error occured" }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetCommentsByTopicID(int topicID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByTopicID?TopicID={0}", topicID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var comments = Task.Run<List<CommentModel>>(() => HttpClientBuilder<CommentModel>.GetListAsync(url, token)).Result;
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
				url = string.Format("http://localhost:8080/api/Section/GetAnonymousUsers?TopicID={0}", topicID);
				var userIDs = Task.Run<List<int>>(() => HttpClientBuilder<int>.GetListAsync(url, token)).Result;
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.TopicID = topicID;
				ViewBag.IP = Request.UserHostAddress;
				ViewBag.UserIDs = userIDs;

				if (page == null)
					return View(viewModels.ToPagedList(1, 5));
				else
					return View(viewModels.ToPagedList(page.Value, 5));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetCommentsByTopicID");
				return View("Error", info);
			}
		}

		public ActionResult GetCommentsByTopicWithOwnerAndType(int topicID, int ownerID, string topicType, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByTopicID?TopicID={0}", topicID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				//var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var comments = Task.Run<List<CommentModel>>(() => HttpClientBuilder<CommentModel>.GetListAsync(url, token)).Result;
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				ViewBag.User = (UserModel)HttpContext.Session["currentUser"];
				ViewBag.TopicID = topicID;
				ViewBag.OwnerID = ownerID;
				ViewBag.TopicType = topicType;
				ViewBag.IP = Request.UserHostAddress;

				if (page == null)
					return View(viewModels.ToPagedList(1, 5));
				else
					return View(viewModels.ToPagedList(page.Value, 5));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetCommentsByTopicID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetCommentsByAuthorID(int authorID, int? page = null)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByAuthorID?AuthorID={0}", authorID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }

				if (page == null)
					return View(viewModels.ToPagedList(1, 15));
				else
					return View(viewModels.ToPagedList(page.Value, 15));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetCommentsByAuthorID");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> GetCommentsByAuthorIDAndSectionID(int authorID, int sectionID, int? page = null)
		{
			try
			{
				if (Request.UrlReferrer == null)
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
				
				string url = string.Format("http://localhost:8080/api/Comment/GetCommentsByAuthorIDAndSectionID?AuthorID={0}&SectionID={1}", authorID, sectionID);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				url = string.Format("http://localhost:8080/api/Section/GetSectionAnonymous?SectionID={0}", sectionID);
				var userIDs = Task.Run<List<int>>(() => HttpClientBuilder<int>.GetListAsync(url, token)).Result;
				ViewBag.AuthorID = authorID;
				ViewBag.SectionID = sectionID;
				ViewBag.UserIDs = userIDs;

				if (page == null)
					return View(viewModels.ToPagedList(1, 5));
				else
					return View(viewModels.ToPagedList(page.Value, 5));
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetCommentsByAuthorIDAndSectionID");
				return View("Error", info);
			}
		}

        public async Task<ActionResult> GetReportedComments(int sectionID, int? page = null)
        {
            try
            {
                string url = string.Format("http://localhost:8080/api/Comment/GetReportedComments?SectionID={0}", sectionID);
                string token = "";
                if (HttpContext.Session["token"] != null)
                    token = HttpContext.Session["token"].ToString();
                else
                    token = null;
                var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
                ViewBag.SectionID = sectionID;

                if (page == null)
                    return View(viewModels.ToPagedList(1, 15));
                else
                    return View(viewModels.ToPagedList(page.Value, 15));
            }
            catch (Exception ex)
            {
                HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "GetReportedComments");
                return View("Error", info);
            }
        }

		[HttpGet]
		public async Task<ActionResult> SearchByTextContent(string content)
		{
			try
			{
				string url = string.Format("http://localhost:8080/api/Comment/SearchByTextContent?Content={0}", content);
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var comments = await HttpClientBuilder<CommentModel>.GetListAsync(url, token);
                var viewModels = new List<CommentViewModel>();
                foreach (CommentModel c in comments)
                {
                    viewModels.Add(CommentConverter.FromBasicToVisual(c));
                }
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "SearchByTextContent");
				return View("Error", info);
			}
		}

		public async Task<ActionResult> SearchCommentsByCriteria(CommentCriteriaViewModel criteria)
		{
			try
			{
				string url = "http://localhost:8080/api/Comment/SearchCommentsByCriteria";
				string token = "";
				if (HttpContext.Session["token"] != null)
					token = HttpContext.Session["token"].ToString();
				else
					token = null;
				var commentCriteria = CommentConverter.CriteriaFromVisualToBasic(criteria);
				var comments = await HttpClientBuilder<CommentCriteria>.GetListAsync<CommentModel>(commentCriteria, url, token);
				var viewModels = new List<CommentViewModel>();
				foreach (CommentModel c in comments)
				{
					viewModels.Add(CommentConverter.FromBasicToVisual(c));
				}
				return View();
			}
			catch (Exception ex)
			{
				HandleErrorInfo info = new HandleErrorInfo(ex, "Comment", "SearchCommentsByCriteria");
				return View("Error", info);
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateRating(int commentID, int voterID, bool like)
		{
			try
			{
				if (!Request.IsAjaxRequest())
					return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

				string url = string.Format("http://localhost:8080/api/Comment/UpdateRating?CommentID={0}&VoterID={1}&Like={2}", commentID, voterID, like);
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
        public async Task<ActionResult> ReportComment(int commentID, bool isReported)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                string url = string.Format("http://localhost:8080/api/Comment/ReportComment?CommentID={0}&IsReported={1}", commentID, isReported);
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
    }
}