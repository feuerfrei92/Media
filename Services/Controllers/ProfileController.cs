using Data;
using Models;
using Models.DatabaseModels;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
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

			var newTopic = new Topic
			{
				Name = profile.Name,
				SectionID = newProfile.ID,
				AuthorID = userID,
				DateCreated = DateTime.Now,
				IsProfileTopic = true,
				IsInterestTopic = false,
			};

			_nest.Topics.Create(newTopic);

			var newSetting = new Setting
			{
				OwnerID = newProfile.ID,
				OwnerType = "Profile",
				Publicity = "Everyone",
			};

			_nest.Settings.Create(newSetting);

			var newTopicSetting = new Setting
			{
				OwnerID = newTopic.ID,
				OwnerType = "Topic",
				Publicity = "Everyone",
			};

			_nest.Settings.Create(newTopicSetting);

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
		public IHttpActionResult GetCommonFriends(int userID, int targetID)
		{
			List<Friendship> friendshipsUser = _nest.Friendships.All().Where(f => (f.UserID_1 == userID || f.UserID_2 == userID) && f.IsAccepted == true)
			.ToList();
			List<Friendship> friendshipsTarget = _nest.Friendships.All().Where(f => (f.UserID_1 == targetID || f.UserID_2 == targetID) && f.IsAccepted == true)
			.ToList();
			
			var profilesUser = new List<ProfileModel>();
			foreach (Friendship f in friendshipsUser)
			{
				if (f.UserID_1 == userID)
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_2).Select(BuildProfileModel).FirstOrDefault();
					profilesUser.Add(profile);
				}
				else
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_1).Select(BuildProfileModel).FirstOrDefault();
					profilesUser.Add(profile);
				}
			}

			var profilesTarget = new List<ProfileModel>();

			foreach (Friendship f in friendshipsTarget)
			{
				if (f.UserID_1 == targetID)
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_2).Select(BuildProfileModel).FirstOrDefault();
					profilesTarget.Add(profile);
				}
				else
				{
					ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_1).Select(BuildProfileModel).FirstOrDefault();
					profilesTarget.Add(profile);
				}
			}

			var commonProfiles = profilesUser.Intersect(profilesTarget).ToList();
			return Ok(commonProfiles);
		}

		[HttpGet]
		public IHttpActionResult GetPendingFriends(int userID)
		{
			List<Friendship> friendships = _nest.Friendships.All().Where(f => f.UserID_2 == userID && f.IsAccepted == false)
			.ToList();
			var profiles = new List<ProfileModel>();
			foreach (Friendship f in friendships)
			{
				ProfileModel profile = _nest.Profiles.All().Where(p => p.UserID == f.UserID_1).Select(BuildProfileModel).FirstOrDefault();
				profiles.Add(profile);
			}
			return Ok(profiles);
		}

		[HttpGet]
		public IHttpActionResult GetFriendship(int userID, int friendID)
		{
			Friendship friendship = _nest.Friendships.All().Where(f => (f.UserID_1 == userID && f.UserID_2 == friendID) || (f.UserID_1 == friendID && f.UserID_2 == userID)).FirstOrDefault();

			var friendshipInfo = new FriendshipInfo
			{
				ID = friendship.ID,
				UserID_1 = friendship.UserID_1,
				UserID_2 = friendship.UserID_2,
				IsAccepted = friendship.IsAccepted,
			};

			return Ok(friendshipInfo);
		}

		[HttpGet]
		public IHttpActionResult GetLatestProfileActivity(int userID)
		{
			var activities = new List<ActivityModel>();
			User user = _nest.Users.All().Where(u => u.ID == userID).FirstOrDefault();
			var userModel = new UserModel
			{
				ID = user.ID,
				Username = user.Username,
			};
			activities.AddRange(GetLatestActivities(userModel));

			return Ok(activities);
		}

		[HttpGet]
		public IHttpActionResult GetLatestFriendsActivity(int userID)
		{
			try
			{
				List<Friendship> friendships = _nest.Friendships.All().Where(f => (f.UserID_1 == userID || f.UserID_2 == userID) && f.IsAccepted == true)
				.ToList();
				var activities = new List<ActivityModel>();
				foreach (Friendship f in friendships)
				{
					if (f.UserID_1 == userID)
					{
						User user = _nest.Users.All().Where(u => u.ID == f.UserID_2).FirstOrDefault();
						var userModel = new UserModel
						{
							ID = user.ID,
							Username = user.Username,
						};
						activities.AddRange(GetLatestActivities(userModel));
					}
					else
					{
						User user = _nest.Users.All().Where(u => u.ID == f.UserID_1).FirstOrDefault();
						var userModel = new UserModel
						{
							ID = user.ID,
							Username = user.Username,
						};
						activities.AddRange(GetLatestActivities(userModel));
					}
				}

				activities = activities.OrderByDescending(a => a.DateCreated).Take(200).ToList();

				return Ok(activities);
			}
			catch
			{
				throw;
			}
		}

		[HttpPost]
		public IHttpActionResult CreateMessage(int senderID, int receiverID, MessageModel message)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var text = message.Text;
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(text));
			doc.Load(ms);
			doc.DocumentNode.Descendants()
				.Where(n => n.Name == "script" || n.Name == "object" || n.Name == "embed" || n.Name == "link")
				.ToList()
				.ForEach(n => n.Remove());
			var cleanText = doc.DocumentNode.OuterHtml;

			var newMessage = new Message
			{
				SenderID = senderID,
				ReceiverID = receiverID,
				Text = cleanText,
				DateCreated = DateTime.Now,
			};

			_nest.Messages.Create(newMessage);


			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			message.ID = newMessage.ID;

			return Ok(message);
		}

		public IHttpActionResult GetMessages(int senderID, int receiverID)
		{
			var messages = _nest.Messages.All().Where(m => (m.SenderID == senderID && m.ReceiverID == receiverID) || (m.SenderID == receiverID && m.ReceiverID == senderID))
				.Select(m => new MessageModel { ID = m.ID, SenderID = m.SenderID, ReceiverID = m.ReceiverID, Text = m.Text, DateCreated = m.DateCreated, DiscussionGuid = m.DiscussionGuid })
				.OrderByDescending(m => m.DateCreated)
				.ToList();

			return Ok(messages);
		}

		public IHttpActionResult GetDiscussion(Guid discussionGuid)
		{
			var messages = _nest.Messages.All().Where(m => m.DiscussionGuid == discussionGuid)
				.Select(m => new MessageModel { ID = m.ID, SenderID = m.SenderID, ReceiverID = m.ReceiverID, Text = m.Text, DateCreated = m.DateCreated, DiscussionGuid = m.DiscussionGuid })
				.OrderByDescending(m => m.DateCreated)
				.ToList();

			return Ok(messages);
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

		private List<ActivityModel> GetLatestActivities(UserModel user)
		{
			var activities = new List<ActivityModel>();
			List<Topic> topics = _nest.Topics.All().Where(t => t.AuthorID == user.ID).OrderByDescending(t => t.DateCreated).Take(100).ToList();
			foreach (Topic t in topics)
			{
				activities.Add(new ActivityModel { ActionID = t.ID, Author = user, Action = "topic", DateCreated = t.DateCreated });
			}
			List<Comment> comments = _nest.Comments.All().Where(c => c.AuthorID == user.ID).OrderByDescending(c => c.DateCreated).Take(100).ToList();
			foreach (Comment c in comments)
			{
				activities.Add(new ActivityModel { ActionID = c.TopicID, Author = user, Action = "comment", DateCreated = c.DateCreated });
			}
			List<int> albumsIDs = _nest.Albums.All().Where(a => a.OwnerID == user.ID).Select(a => a.ID).ToList();
			List<PhotoModel> photos = _nest.Photos.All().Select(p => new PhotoModel { ID = p.ID, AlbumID = p.AlbumID, Content = p.Content, DateCreated = p.DateCreated, Rating = p.Rating }).ToList();
			photos = photos.Where(p => albumsIDs.Exists(a => a == p.AlbumID)).OrderByDescending(p => p.DateCreated).Take(100).ToList();
			foreach (PhotoModel p in photos)
			{
				activities.Add(new ActivityModel { ActionID = p.ID, Author = user, Action = "photo", DateCreated = p.DateCreated });
			}
			List<Video> videos = _nest.Videos.All().Where(v => v.OwnerID == user.ID).OrderByDescending(v => v.DateCreated).Take(100).ToList();
			foreach (Video v in videos)
			{
				activities.Add(new ActivityModel { ActionID = v.ID, Author = user, Action = "video", DateCreated = v.DateCreated });
			}

			activities = activities.OrderByDescending(a => a.DateCreated).Take(100).ToList();
			return activities;
		}
	}
}
