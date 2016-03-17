using Data;
using Models;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Services.Controllers
{
    public class CommentController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.Comment, CommentModel>> BuildCommentModel
		{
			get { return c => new CommentModel { ID = c.ID, Name = c.Name, Text = c.Text, TopicID = c.TopicID, AuthorID = c.AuthorID, DateCreated = c.DateCreated, DateModified = c.DateModified, Rating = c.Rating }; }
		}

		public CommentController()
			: this(new RepositoryNest())
		{

		}

		public CommentController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAllComments()
		{
			IQueryable<CommentModel> comments = _nest.Comments.All().Select(BuildCommentModel);
			return Ok(comments);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateComment(int ID, int topicID, CommentModel comment)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Comment existingComment = _nest.Comments.All().Where(c => c.ID == ID).FirstOrDefault();
			
			if(existingComment == null)
			{
				return BadRequest("No comment with the specified ID exists");
			}

			existingComment.Name = comment.Name;
			existingComment.Text = comment.Text;
			existingComment.TopicID = topicID;
			existingComment.DateCreated = comment.DateCreated;
			existingComment.DateModified = DateTime.Now;

			_nest.Comments.Update(existingComment);

			comment.ID = ID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(comment);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateComment(int topicID, int authorID, CommentModel comment)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var text = comment.Text;
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
			doc.Load(ms);
			doc.DocumentNode.Descendants()
				.Where(n => n.Name == "script" || n.Name == "object" || n.Name == "embed" || n.Name == "link")
				.ToList()
				.ForEach(n => n.Remove());
			var cleanText = doc.DocumentNode.OuterHtml;

			var newComment = new Comment
			{
				Name = comment.Name,
				Text = cleanText,
				TopicID = topicID,
				AuthorID = authorID,
				DateCreated = DateTime.Now,
				Rating = 0,
			};

			_nest.Comments.Create(newComment);

			var topic = _nest.Topics.All().Where(t => t.ID == topicID).FirstOrDefault();
			topic.DateModified = newComment.DateCreated;

			_nest.Topics.Update(topic);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			comment.ID = newComment.ID;
			comment.AuthorID = newComment.AuthorID;
			comment.TopicID = newComment.TopicID;

			return Ok(comment);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteComment(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Comment existingComment = _nest.Comments.All().Where(c => c.ID == ID).FirstOrDefault();

			if (existingComment == null)
			{
				return BadRequest("No comment with the specified ID exists");
			}

			_nest.Comments.Delete(existingComment);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok();
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetCommentByID(int ID)
		{
			CommentModel comment = _nest.Comments.All().Where(c => c.ID == ID).Select(BuildCommentModel).FirstOrDefault();

			if (comment == null)
			{
				return BadRequest("No comment with the specified ID exists.");
			}

			return Ok(comment);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetCommentsByTopicID(int topicID)
		{
			List<CommentModel> comments = _nest.Comments.All().Where(c => c.TopicID == topicID).Select(BuildCommentModel).ToList();

			return Ok(comments);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetCommentsByAuthorID(int authorID)
		{
			List<CommentModel> comments = _nest.Comments.All().Where(c => c.AuthorID == authorID).Select(BuildCommentModel).ToList();

			return Ok(comments);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetCommentsByAuthorIDAndSectionID(int authorID, int sectionID)
		{
			List<CommentModel> comments = _nest.Comments.All().Where(c => c.AuthorID == authorID).Select(BuildCommentModel).ToList();
			var sectionComments = new List<CommentModel>();
			foreach(CommentModel c in comments)
			{
				var topic = _nest.Topics.All().Where(t => t.ID == c.TopicID).FirstOrDefault();
				if (topic.SectionID == sectionID)
					sectionComments.Add(c);
			}

			return Ok(sectionComments);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult SearchByTextContent(string content)
		{
			List<CommentModel> comments = _nest.Comments.All().Where(c => c.Text.Contains(content)).Select(BuildCommentModel).ToList();

			return Ok(comments);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult SearchByCriteria(CommentCriteria criteria)
		{
			List<CommentModel> comments = _nest.Comments.All().Where(c => DoesMatchCriteria(c, criteria)).Select(BuildCommentModel).ToList();

			return Ok(comments);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateRating(int commentID, bool like)
		{
			Comment comment = _nest.Comments.All().Where(c => c.ID == commentID).FirstOrDefault();

			if (like)
				comment.Rating++;
			else
				comment.Rating--;

			_nest.Comments.Update(comment);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok();
		}

		private bool DoesMatchCriteria(Comment comment, CommentCriteria criteria)
		{
			if (!comment.Name.Contains(criteria.Name))
				return false;
			if (criteria.TopicID != null && !(comment.TopicID == criteria.TopicID))
				return false;
			if (criteria.AuthorID != null && !(comment.AuthorID == criteria.AuthorID))
				return false;
			if (criteria.DateCreatedFrom != null && comment.DateCreated.CompareTo(criteria.DateCreatedFrom) == -1)
				return false;
			if (criteria.DateCreatedTo != null && comment.DateCreated.CompareTo(criteria.DateCreatedTo) == 1)
				return false;
			if (comment.DateModified != null)
			{
				if (criteria.DateModifiedFrom != null && comment.DateModified.Value.CompareTo(criteria.DateModifiedFrom) == -1)
					return false;
				if (criteria.DateModifiedTo != null && comment.DateModified.Value.CompareTo(criteria.DateModifiedTo) == 1)
					return false;
			}

			return true;
		}
    }
}
