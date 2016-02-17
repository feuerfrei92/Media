using Data;
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
using Microsoft.AspNet.Identity;

namespace Services.Controllers
{
    public class UserController : ApiController 
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.User, UserModel>> BuildUserModel
		{
			get { return u => new UserModel { ID = u.ID, Username = u.Username }; }
		}

		public UserController()
			: this(new RepositoryNest())
		{

		}

		public UserController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		public IHttpActionResult GetAllUsers()
		{
			IQueryable<UserModel> users = _nest.Users.All().Select(BuildUserModel);
			return Ok(users);
		}

		[HttpGet]
		public IHttpActionResult GetCurrentUser()
		{
			UserModel currentUser = _nest.Users.All().Where(u => u.Username == User.Identity.Name).Select(BuildUserModel).FirstOrDefault();
			return Ok(currentUser);
		}

		[HttpPut]
		public IHttpActionResult UpdateUser(int ID, UserModel user)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			User existingUser = _nest.Users.All().Where(u => u.ID == ID).FirstOrDefault();
			
			if(existingUser == null)
			{
				return BadRequest("No user with the specified ID exists.");
			}

			existingUser.Username = user.Username;
			_nest.Users.Update(existingUser);

			user.ID = ID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(user);
		}

        [HttpPut]
        public IHttpActionResult ChangeActivity(int ID)
        {
            if (!(ModelState.IsValid))
            {
                return BadRequest(ModelState);
            }

            User existingUser = _nest.Users.All().Where(u => u.ID == ID).FirstOrDefault();

            if (existingUser == null)
            {
                return BadRequest("No user with the specified ID exists.");
            }

            existingUser.IsActive = !existingUser.IsActive;
            _nest.Users.Update(existingUser);

            try
            {
                _nest.SaveChanges();
            }
            catch
            {
                throw;
            }

            return Ok(existingUser);
        }

		[HttpPost]
		public IHttpActionResult CreateUser(UserModel user)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newUser = new User
			{
				Username = user.Username,
                IsActive = true,
			};

			_nest.Users.Create(newUser);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			user.ID = newUser.ID;

			return Ok(user);
		}

		[HttpDelete]
		public IHttpActionResult DeleteUser(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			User existingUser = _nest.Users.All().Where(u => u.ID == ID).FirstOrDefault();

			if (existingUser == null)
			{
				return BadRequest("No user with the specified ID exists.");
			}

			_nest.Users.Delete(existingUser);

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
		public IHttpActionResult GetUserByID(int ID)
		{
			UserModel user = _nest.Users.All().Where(u => u.ID == ID).Select(BuildUserModel).FirstOrDefault();
			
			if (user == null)
			{
				return BadRequest("No user with the specified ID exists.");
			}
			
			return Ok(user);
		}

		[HttpGet]
		public IHttpActionResult GetUserByUsername(string username)
		{
			UserModel user = _nest.Users.All().Where(u => u.Username == username).Select(BuildUserModel).FirstOrDefault();

			if (user == null)
			{
				return BadRequest("No user with the specified username exists.");
			}

			return Ok(user);
		}

		[HttpGet]
		public IHttpActionResult SearchUsernameByString(string partialUsername)
		{
			List<UserModel> users = _nest.Users.All().Where(u => u.Username.Contains(partialUsername)).Select(BuildUserModel).ToList();

			return Ok(users);
		}

		[HttpPost]
		public IHttpActionResult CreateVisit(int userID, int topicID)
		{
			var newVisit = new Visit
			{
				UserID = userID,
				TopicID = topicID,
				LastVisit = DateTime.Now,
			};

			_nest.Visits.Create(newVisit);

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

		[HttpPut]
		public IHttpActionResult UpdateVisit(int userID, int topicID)
		{
			Visit visit = _nest.Visits.All().Where(v => v.UserID == userID && v.TopicID == topicID).FirstOrDefault();

			if (visit == null)
			{
				return BadRequest("No visit with the specified ID exists.");
			}

			visit.LastVisit = DateTime.Now;

			_nest.Visits.Update(visit);

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
		public IHttpActionResult GetVisitsByUserID(int userID)
		{
			List<Visit> visits = _nest.Visits.All().Where(v => v.UserID == userID).ToList();

			return Ok(visits);
		}

		[HttpGet]
		public IHttpActionResult GetVisitsByTopicID(int topicID)
		{
			List<Visit> visits = _nest.Visits.All().Where(v => v.TopicID == topicID).ToList();

			return Ok(visits);
		}
    }
}
