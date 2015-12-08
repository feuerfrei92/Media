using Data;
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
    public class SectionController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.Section, SectionModel>> BuildSectionModel
		{
			get { return s => new SectionModel { ID = s.ID, Name = s.Name }; }
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
		public IHttpActionResult GetSectionByParentID(int parentID)
		{
			SectionModel section = _nest.Sections.All().Where(s => s.ParentSectionID == parentID).Select(BuildSectionModel).FirstOrDefault();

			if (section == null)
			{
				return BadRequest("No section with the specified parent ID exists.");
			}

			return Ok(section);
		}
	}
}
