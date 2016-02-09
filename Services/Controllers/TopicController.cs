﻿using Data;
using Models;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Services.Controllers
{
    public class TopicController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.Topic, TopicModel>> BuildTopicModel
		{
			get { return t => new TopicModel { ID = t.ID, Name = t.Name, AuthorID = t.AuthorID, DateCreated = t.DateCreated, DateModified = t.DateModified }; }
		}

		public TopicController()
			: this(new RepositoryNest())
		{

		}

		public TopicController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		public IHttpActionResult GetAllTopics()
		{
			IQueryable<TopicModel> topics = _nest.Topics.All().Select(BuildTopicModel);
			return Ok(topics);
		}

		[HttpPut]
		public IHttpActionResult UpdateTopic(int ID, int sectionID, TopicModel topic)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Topic existingTopic = _nest.Topics.All().Where(p => p.ID == ID).FirstOrDefault();
			
			if(existingTopic == null)
			{
				return BadRequest("No topic with the specified ID exists");
			}

			existingTopic.Name = topic.Name;
			existingTopic.SectionID = sectionID;
			existingTopic.DateCreated = topic.DateCreated;
			existingTopic.DateModified = DateTime.Now;

			_nest.Topics.Update(existingTopic);

			topic.ID = ID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(topic);
		}

		[HttpPost]
		public IHttpActionResult CreateTopic(int sectionID, int authorID, TopicModel topic)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newTopic = new Topic
			{
				Name = topic.Name,
				SectionID = sectionID,
				AuthorID = authorID,
				DateCreated = DateTime.Now,
			};

			_nest.Topics.Create(newTopic);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			topic.ID = newTopic.ID;
			topic.ID = newTopic.AuthorID;

			return Ok(topic);
		}

		[HttpDelete]
		public IHttpActionResult DeleteTopic(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Topic existingTopic = _nest.Topics.All().Where(p => p.ID == ID).FirstOrDefault();

			if (existingTopic == null)
			{
				return BadRequest("No topic with the specified ID exists");
			}

			_nest.Topics.Delete(existingTopic);

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
		public IHttpActionResult GetTopicByID(int ID)
		{
			TopicModel topic = _nest.Topics.All().Where(t => t.ID == ID).Select(BuildTopicModel).FirstOrDefault();

			if (topic == null)
			{
				return BadRequest("No topic with the specified ID exists.");
			}

			return Ok(topic);
		}

		[HttpGet]
		public IHttpActionResult GetTopicsBySectionID(int sectionID)
		{
			List<TopicModel> topics = _nest.Topics.All().Where(t => t.SectionID == sectionID).Select(BuildTopicModel).ToList();

			return Ok(topics);
		}

		[HttpGet]
		public IHttpActionResult GetTopicsByAuthorID(int authorID)
		{
			List<TopicModel> topics = _nest.Topics.All().Where(t => t.AuthorID == authorID).Select(BuildTopicModel).ToList();

			return Ok(topics);
		}

		[HttpGet]
		public IHttpActionResult SearchByTopicName(string name)
		{
			List<TopicModel> topics = _nest.Topics.All().Where(t => t.Name.Contains(name)).Select(BuildTopicModel).ToList();

			return Ok(topics);
		}

		[HttpPost]
		public IHttpActionResult SearchByCriteria(TopicCriteria criteria)
		{
			List<TopicModel> topics = _nest.Topics.All().Where(t => DoesMatchCriteria(t, criteria)).Select(BuildTopicModel).ToList();

			return Ok(topics);
		}

		private bool DoesMatchCriteria(Topic topic, TopicCriteria criteria)
		{
			if (!topic.Name.Contains(criteria.Name))
				return false;
			if (criteria.SectionID != null && !(topic.SectionID == criteria.SectionID))
				return false;
			if (criteria.AuthorID != null && !(topic.AuthorID == criteria.AuthorID))
				return false;
			if (criteria.DateCreatedFrom != null && topic.DateCreated.CompareTo(criteria.DateCreatedFrom) == -1)
				return false;
			if (criteria.DateCreatedTo != null && topic.DateCreated.CompareTo(criteria.DateCreatedTo) == 1)
				return false;
			if (topic.DateModified != null)
			{
				if (criteria.DateModifiedFrom != null && topic.DateModified.Value.CompareTo(criteria.DateModifiedFrom) == -1)
					return false;
				if (criteria.DateModifiedTo != null && topic.DateModified.Value.CompareTo(criteria.DateModifiedTo) == 1)
					return false;
			}

			return true;
		}
    }
}
