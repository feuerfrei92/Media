﻿using Data;
using Models;
using Models.DatabaseModels;
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
    public class InterestController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Interest, InterestModel>> BuildInterestModel
		{
			get { return i => new InterestModel { ID = i.ID, Name = i.Name, AuthorID = i.AuthorID, PictureID = i.PictureID }; }
		}

		public InterestController()
			: this(new RepositoryNest())
		{

		}

		public InterestController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAllInterests()
		{
			IQueryable<InterestModel> interests = _nest.Interests.All().Select(BuildInterestModel);
			return Ok(interests);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateInterest(int ID, InterestModel interest)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Interest existingInterest = _nest.Interests.All().Where(i => i.ID == ID).FirstOrDefault();
			
			if(existingInterest == null)
			{
				return BadRequest("No interest with the specified ID exists.");
			}

			existingInterest.Name = interest.Name;
			_nest.Interests.Update(existingInterest);

			interest.ID = ID;
			interest.AuthorID = existingInterest.AuthorID;
			interest.PictureID = existingInterest.PictureID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(interest);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateInterest(int authorID, InterestModel interest)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newInterest = new Interest
			{
				Name = interest.Name,
				AuthorID = authorID,
			};

			_nest.Interests.Create(newInterest);

			var newTopic = new Topic
			{
				Name = interest.Name,
				SectionID = newInterest.ID,
				AuthorID = authorID,
				DateCreated = DateTime.Now,
				TopicType = "Interest",
			};

			_nest.Topics.Create(newTopic);

			var newSetting = new Setting
			{
				OwnerID = newInterest.ID,
				OwnerType = "Interest",
				Publicity = "Everyone",
			};

			_nest.Settings.Create(newSetting);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			interest.ID = newInterest.ID;
			interest.AuthorID = newInterest.AuthorID;

			return Ok(interest);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteInterest(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Interest existingInterest = _nest.Interests.All().Where(i => i.ID == ID).FirstOrDefault();

			if (existingInterest == null)
			{
				return BadRequest("No interest with the specified ID exists.");
			}

			_nest.Interests.Delete(existingInterest);

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
		public IHttpActionResult GetInterestByID(int ID)
		{
			InterestModel interest = _nest.Interests.All().Where(i => i.ID == ID).Select(BuildInterestModel).FirstOrDefault();
			
			if (interest == null)
			{
				return BadRequest("No interest with the specified ID exists.");
			}
			
			return Ok(interest);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetInterestByName(string name)
		{
			InterestModel interest = _nest.Interests.All().Where(i => i.Name == name).Select(BuildInterestModel).FirstOrDefault();

			if (interest == null)
			{
				return BadRequest("No interest with the specified username exists.");
			}

			return Ok(interest);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult SearchInterestNameByString(string partialName)
		{
			List<InterestModel> interests = _nest.Interests.All().Where(i => i.Name.Contains(partialName)).Select(BuildInterestModel).ToList();

			return Ok(interests);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetInterestsForUser(int userID)
		{
			List<Follower> followers = _nest.Followers.All().Where(f => f.UserID == userID).ToList();
			var interests = new List<InterestModel>();
			foreach (Follower f in followers)
			{
				InterestModel interest = _nest.Interests.All().Where(i => i.ID == f.InterestID).Select(BuildInterestModel).FirstOrDefault();
				interests.Add(interest);
			}
			return Ok(interests);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetFollower(int userID, int interestID)
		{
			Follower follower = _nest.Followers.All().Where(f => f.UserID == userID && f.InterestID == interestID).FirstOrDefault();

			var followerInfo = new FollowerInfo
			{
				ID = follower.ID,
				UserID = follower.UserID,
				InterestID = follower.InterestID,
			};

			return Ok(followerInfo);
		}
    }
}
