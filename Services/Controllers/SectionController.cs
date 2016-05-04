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
    public class SectionController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.Section, SectionModel>> BuildSectionModel
		{
			get { return s => new SectionModel { ID = s.ID, Name = s.Name, ParentID = s.ParentSectionID }; }
		}

		public SectionController()
			: this(new RepositoryNest())
		{

		}

		public SectionController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAllSections()
		{
			IQueryable<SectionModel> sections = _nest.Sections.All().Select(BuildSectionModel);
			return Ok(sections);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateSection(int ID, SectionModel section, int? parentID = null)
		{
			if(!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Section existingSection = _nest.Sections.All().Where(s => s.ID == ID).FirstOrDefault();
			
			if(existingSection == null)
			{
				return BadRequest("No section with the specified ID exists");
			}

			existingSection.Name = section.Name;
			existingSection.ParentSectionID = parentID;

			_nest.Sections.Update(existingSection);

			section.ID = ID;
			section.ParentID = parentID;

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			return Ok(section);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateSection(SectionModel section, int authorID, int? parentID = null)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newSection = new Section
			{
				Name = section.Name,
				ParentSectionID = parentID,
			};

			_nest.Sections.Create(newSection);

			var newSetting = new Setting
			{
				OwnerID = newSection.ID,
				OwnerType = "Section",
				Publicity = "Everyone",
			};

			_nest.Settings.Create(newSetting);

			if (parentID == null)
			{
				var newMembership = new Membership
				{
					SectionID = newSection.ID,
					UserID = authorID,
					IsAccepted = true,
					Role = SectionRole.Admin,
					SuspendedUntil = null,
					Anonymous = false,
				};

				_nest.Memberships.Create(newMembership);
			}

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			section.ID = newSection.ID;

			return Ok(section);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteSection(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Section existingSection = _nest.Sections.All().Where(s => s.ID == ID).FirstOrDefault();

			if (existingSection == null)
			{
				return BadRequest("No section with the specified ID exists");
			}

			_nest.Sections.Delete(existingSection);

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
		public IHttpActionResult GetSectionByID(int ID)
		{
			SectionModel section = _nest.Sections.All().Where(s => s.ID == ID).Select(BuildSectionModel).FirstOrDefault();

			if (section == null)
			{
				return BadRequest("No section with the specified ID exists.");
			}

			return Ok(section);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetSectionsByParentID(int parentID)
		{
			List<SectionModel> sections = _nest.Sections.All().Where(s => s.ParentSectionID == parentID).Select(BuildSectionModel).ToList();

			//if (sections == null)
			//{
			//	return BadRequest("No section with the specified parent ID exists.");
			//}

			return Ok(sections);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetRoot(int sectionID)
		{
			SectionModel section = GetSectionRoot(sectionID);

			return Ok(section);
		}

		[HttpGet]
		//[Authorize]
		public IHttpActionResult SearchBySectionName(string name)
		{
			List<SectionModel> sections = _nest.Sections.All().Where(s => s.Name.Contains(name)).Select(BuildSectionModel).ToList();

			return Ok(sections);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult AddMembership(int sectionID, int userID, bool isAnonymous)
		{
			var membership = new Membership
			{
				SectionID = sectionID,
				UserID = userID,
				Role = SectionRole.Regular,
				IsAccepted = false,
				Anonymous = isAnonymous,
			};

			_nest.Memberships.Create(membership);

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
		public IHttpActionResult AcceptMembership(int sectionID, int userID)
		{
			var membership = _nest.Memberships.All().Where(m => m.SectionID == sectionID && m.UserID == userID).FirstOrDefault();
			membership.IsAccepted = true;

			_nest.Memberships.Update(membership);

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
		public IHttpActionResult ChangeVisibilityOfMembership(int sectionID, int userID)
		{
			var membership = _nest.Memberships.All().Where(m => m.SectionID == sectionID && m.UserID == userID).FirstOrDefault();
			if (membership.Anonymous)
				membership.Anonymous = false;
			else
				membership.Anonymous = true;

			_nest.Memberships.Update(membership);

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
		public IHttpActionResult UpdateMembership(int sectionID, int userID, string role, DateTime? suspension = null)
		{
			var membership = _nest.Memberships.All().Where(m => m.SectionID == sectionID && m.UserID == userID).FirstOrDefault();
			membership.Role = (SectionRole)Enum.Parse(typeof(SectionRole), role);
			membership.SuspendedUntil = suspension;

			_nest.Memberships.Update(membership);

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
		public IHttpActionResult DeleteMembership(int sectionID, int userID)
		{
			var membership = _nest.Memberships.All().Where(m => m.SectionID == sectionID && m.UserID == userID).FirstOrDefault();

			_nest.Memberships.Delete(membership);

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
		public IHttpActionResult GetMembershipsForUser(int userID)
		{
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.UserID == userID).ToList();
			var sections = new List<SectionModel>();
			foreach(Membership m in memberships)
			{
				SectionModel section = _nest.Sections.All().Where(s => s.ID == m.SectionID).Select(BuildSectionModel).FirstOrDefault();
				sections.Add(section);
			}

			return Ok(sections);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetMembership(int userID, int sectionID)
		{
			Membership membership = _nest.Memberships.All().Where(m => m.UserID == userID && m.SectionID == sectionID).FirstOrDefault();

			var membershipInfo = new MembershipInfo
			{
				ID = membership.ID,
				UserID = membership.UserID,
				SectionID = membership.SectionID,
				Role = membership.Role,
				SuspendedUntil = membership.SuspendedUntil,
				IsAccepted = membership.IsAccepted,
				Anonymous = membership.Anonymous,
			};

			return Ok(membershipInfo);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAnonymousUsers(int topicID)
		{
			Topic topic = _nest.Topics.All().Where(t => t.ID == topicID).FirstOrDefault();
			SectionModel root = GetSectionRoot(topic.SectionID);
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.SectionID == root.ID).ToList();
			List<int> userIDs = new List<int>();
			foreach (Membership mem in memberships)
			{
				if (mem.Anonymous)
					userIDs.Add(mem.UserID);
			}
			return Ok(userIDs);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetSectionAnonymous(int sectionID)
		{
			SectionModel root = GetSectionRoot(sectionID);
			List<Membership> memberships = _nest.Memberships.All().Where(m => m.SectionID == root.ID).ToList();
			List<int> userIDs = new List<int>();
			foreach (Membership mem in memberships)
			{
				if (mem.Anonymous)
					userIDs.Add(mem.UserID);
			}
			return Ok(userIDs);
		}

		private SectionModel GetSectionRoot(int sectionID)
		{
			SectionModel section = _nest.Sections.All().Where(s => s.ID == sectionID).Select(BuildSectionModel).FirstOrDefault();
			int? parentID = section.ParentID;
			while (parentID != null)
			{
				section = _nest.Sections.All().Where(s => s.ID == parentID).Select(BuildSectionModel).FirstOrDefault();
				parentID = section.ParentID;
			}
			return section;
		}
	}
}
