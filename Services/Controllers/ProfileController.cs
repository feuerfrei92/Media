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

namespace Services.Controllers
{
    public class ProfileController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.Profile, ProfileModel>> BuildProfileModel
		{
			get { return p => new ProfileModel { ID = p.ID, Username = p.Username, Name = p.Name, Age = p.Age, }; }
												//Gender = p.Gender }; }
		}

		public ProfileController()
			: this(new RepositoryNest())
		{

		}

		public ProfileController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		public IHttpActionResult GetAllProfiles()
		{
			IQueryable<ProfileModel> profiles = _nest.Profiles.All().Select(BuildProfileModel);
			return Ok(profiles);
		}

		[HttpPut]
		public IHttpActionResult UpdateProfile(int ID, ProfileModel profile)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Profile existingProfile = _nest.Profiles.All().Where(p => p.ID == ID).FirstOrDefault();

			if (existingProfile == null)
			{
				return BadRequest("No profile with the specified ID exists");
			}

			existingProfile.Name = profile.Name;
			existingProfile.Age = profile.Age;
			existingProfile.Gender = profile.Gender.ToString();

			_nest.Profiles.Update(existingProfile);

			profile.ID = ID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(profile);
		}

		[HttpPost]
		public IHttpActionResult CreateProfile(int userID, ProfileModel profile)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newProfile = new Profile
			{
				UserID = userID,
				Username = profile.Username,
				Name = profile.Name,
				Age = profile.Age,
				Gender = profile.Gender.ToString(),
			};

			_nest.Profiles.Create(newProfile);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			profile.ID = newProfile.ID;

			return Ok(profile);
		}

		[HttpPost]
		public IHttpActionResult AddFriend(int userID, int friendID)
		{
			var newFriendship = new Friendship
			{
				UserID_1 = userID,
				UserID_2 = friendID,
				IsAccepted = false,
			};

			_nest.Friendships.Create(newFriendship);

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
		public IHttpActionResult AcceptFriendship(int userID, int friendID)
		{
			Friendship friendship = _nest.Friendships.All().Where(f => f.UserID_1 == friendID && f.UserID_2 == userID).FirstOrDefault();
			if (friendship.IsAccepted == true)
				return BadRequest("Friendship is already confirmed");
			else
				friendship.IsAccepted = true;
			
			_nest.Friendships.Update(friendship);

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
		public IHttpActionResult DeleteProfile(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Profile existingProfile = _nest.Profiles.All().Where(p => p.ID == ID).FirstOrDefault();

			if (existingProfile == null)
			{
				return BadRequest("No profile with the specified ID exists.");
			}

			_nest.Profiles.Delete(existingProfile);

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
		public IHttpActionResult GetProfileByID(int ID)
		{
			ProfileModel profile = _nest.Profiles.All().Where(p => p.ID == ID).Select(BuildProfileModel).FirstOrDefault();

			if (profile == null)
			{
				return BadRequest("No profile with the specified ID exists.");
			}

			return Ok(profile);
		}

		[HttpGet]
		public IHttpActionResult GetProfileByUserID(int userID)
		{
			ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == userID).Select(BuildProfileModel).FirstOrDefault();

			if (profile == null)
			{
				return BadRequest("No profile with the specified user ID exists.");
			}

			return Ok(profile);
		}

		[HttpGet]
		public IHttpActionResult GetProfileByUsername(string username)
		{
			try
			{
				ProfileModel profile = _nest.Profiles.All().Where(p => p.Username == username).Select(BuildProfileModel).FirstOrDefault();

				if (profile == null)
				{
					return BadRequest("No profile with the specified username exists.");
				}
				
				return Ok(profile);
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		[HttpPost]
		public IHttpActionResult SearchProfilesByCriteria(ProfileCriteria criteria)
		{
			try
			{
				List<ProfileModel> profiles = _nest.Profiles.All().Select(BuildProfileModel).ToList();
				profiles = profiles.Where(p => DoesMatchCriteria(p, criteria)).ToList();

				return Ok(profiles);
			}
			catch
			{
				throw;
			}
		}

		[HttpGet]
		public IHttpActionResult GetAllFriends(int userID)
		{
			List<Friendship> friendships = _nest.Friendships.All().Where(f => (f.UserID_1 == userID || f.UserID_2 == userID) && f.IsAccepted == true)
			.ToList();
			var profiles = new List<ProfileModel>();
			foreach (Friendship f in friendships)
			{
				if (f.UserID_1 == userID)
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_2).Select(BuildProfileModel).FirstOrDefault();
					profiles.Add(profile);
				}
				else
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_1).Select(BuildProfileModel).FirstOrDefault();
					profiles.Add(profile);
				}
			}
			return Ok(profiles);
		}

		[HttpGet]
		public IHttpActionResult GetPendingFriends(int userID)
		{
			List<Friendship> friendships = _nest.Friendships.All().Where(f => (f.UserID_1 == userID || f.UserID_2 == userID) && f.IsAccepted == false)
			.ToList();
			var profiles = new List<ProfileModel>();
			foreach (Friendship f in friendships)
			{
				ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_2).Select(BuildProfileModel).FirstOrDefault();
				profiles.Add(profile);
			}
			return Ok(profiles);
		}

		private bool DoesMatchCriteria(ProfileModel profile, ProfileCriteria criteria)
		{
			if (!(profile.Name.Contains(criteria.Name)))
				return false;
			if (profile.Age < criteria.MinimumAge || profile.Age > criteria.MaximumAge)
				return false;
			if ((criteria.Gender == GenderCriterion.Female && profile.Gender == Gender.Male)
			|| (criteria.Gender == GenderCriterion.Female && profile.Gender == Gender.Male))
				return false;

			return true;
		}
	}
}
