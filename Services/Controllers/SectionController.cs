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
		public IHttpActionResult GetAllSections()
		{
			IQueryable<SectionModel> sections = _nest.Sections.All().Select(BuildSectionModel);
			return Ok(sections);
		}

		[HttpPut]
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
		public IHttpActionResult CreateSection(SectionModel section, int? parentID = null)
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
		public IHttpActionResult SearchBySectionName(string name)
		{
			List<SectionModel> sections = _nest.Sections.All().Where(s => s.Name.Contains(name)).Select(BuildSectionModel).ToList();

			return Ok(sections);
		}

		[HttpPost]
		public IHttpActionResult AddMembership(int sectionID, int userID)
		{
			var membership = new Membership
			{
				SectionID = sectionID,
				UserID = userID,
				Role = SectionRole.Regular,
				IsAccepted = false,
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
			};

			return Ok(membershipInfo);
		}
	}
}
