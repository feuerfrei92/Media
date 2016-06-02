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
			get { return u => new UserModel { ID = u.ID, Username = u.Username, IsAdmin = u.IsAdmin }; }
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
		[Authorize]
		public IHttpActionResult GetAllUsers()
		{
			IQueryable<UserModel> users = _nest.Users.All().Select(BuildUserModel);
			return Ok(users);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetCurrentUser()
		{
			UserModel currentUser = _nest.Users.All().Where(u => u.Username == User.Identity.Name).Select(BuildUserModel).FirstOrDefault();
			return Ok(currentUser);
		}

		[HttpPut]
		[Authorize]
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
			user.IsAdmin = existingUser.IsAdmin;

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
		[Authorize]
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
		[Authorize]
		public IHttpActionResult CreateUser(UserModel user, bool isAdmin)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newUser = new User
			{
				Username = user.Username,
				IsAdmin = isAdmin,
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
			user.IsAdmin = newUser.IsAdmin;

			return Ok(user);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteUser(int ID)
		{
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public IHttpActionResult SearchUsernameByString(string partialUsername)
		{
			List<UserModel> users = _nest.Users.All().Where(u => u.Username.Contains(partialUsername)).Select(BuildUserModel).ToList();

			return Ok(users);
		}

		[HttpPost]
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public IHttpActionResult GetVisitsByUserID(int userID)
		{
			List<Visit> visits = _nest.Visits.All().Where(v => v.UserID == userID).ToList();

			return Ok(visits);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetVisitsByTopicID(int topicID)
		{
			List<Visit> visits = _nest.Visits.All().Where(v => v.TopicID == topicID).ToList();

			return Ok(visits);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetMembershipsForSection(int sectionID)
		{
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.SectionID == sectionID).ToList();
			var users = new List<UserModel>();
			foreach (Membership m in memberships)
			{
				UserModel user = _nest.Users.All().Where(u => u.ID == m.UserID).Select(BuildUserModel).FirstOrDefault();
				users.Add(user);
			}

			return Ok(users);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetMembershipsForSectionRaw(int sectionID, int getSpecial)
		{
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.SectionID == sectionID).ToList();
			if (getSpecial == 0)
				memberships = memberships.Where(m => m.Role != SectionRole.Admin && m.Role != SectionRole.Mod).ToList();
			var users = new List<UserModel>();
			foreach (Membership m in memberships)
			{
				UserModel user = _nest.Users.All().Where(u => u.ID == m.UserID).Select(BuildUserModel).FirstOrDefault();
				users.Add(user);
			}

			return Ok(users);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetPendingMembershipsForSection(int sectionID)
		{
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.SectionID == sectionID && m.IsAccepted == false).ToList();
			var users = new List<UserModel>();
			foreach (Membership m in memberships)
			{
				UserModel user = _nest.Users.All().Where(u => u.ID == m.UserID).Select(BuildUserModel).FirstOrDefault();
				users.Add(user);
			}

			return Ok(users);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult AddUserToGroup(int userID, int groupID)
		{
			var newDiscussionist = new Discussionist
			{
				DiscussionID = groupID,
				UserID = userID,
			};

			_nest.Discussionists.Create(newDiscussionist);

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

		[HttpDelete]
		[Authorize]
		public IHttpActionResult RemoveUserFromGroup(int userID, int groupID)
		{
			Discussionist discussionist = _nest.Discussionists.All().Where(d => d.UserID == userID && d.DiscussionID == groupID).FirstOrDefault();

			if (discussionist == null)
			{
				return BadRequest("No such member exists.");
			}

			_nest.Discussionists.Delete(discussionist);

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
		public IHttpActionResult GetGroupsForUser(int userID)
		{
			var discussionists = _nest.Discussionists.All().Where(d => d.UserID == userID).ToList();
			List<DiscussionModel> discussions = new List<DiscussionModel>();
			foreach (Discussionist dis in discussionists)
			{
				var group = _nest.Discussions.All().Where(d => d.ID == dis.DiscussionID).Select(d => new DiscussionModel { ID = d.ID, Name = d.Name, DiscussionGuid = d.DiscussionGuid }).FirstOrDefault();
				discussions.Add(group);
			}
			return Ok(discussions);
		}
    }
}
