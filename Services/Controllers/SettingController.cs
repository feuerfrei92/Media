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
    public class SettingController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Setting, SettingModel>> BuildSettingModel
		{
			get { return s => new SettingModel { ID = s.ID, OwnerID = s.OwnerID, OwnerType = s.OwnerType, Publicity = s.Publicity }; }
		}

		public SettingController()
			: this(new RepositoryNest())
		{

		}

		public SettingController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult ChangePublicity(int settingID, string publicity)
		{
			Setting setting = _nest.Settings.All().Where(s => s.ID == settingID).FirstOrDefault();

			if (setting == null)
			{
				return BadRequest("No setting with the specified ID exists");
			}

			setting.Publicity = publicity;

			_nest.Settings.Update(setting);

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

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateSetting(int ownerID, SettingModel setting)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newSetting = new Setting
			{
				OwnerID = ownerID,
				OwnerType = setting.OwnerType.ToString(),
				Publicity = setting.Publicity.ToString(),
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

			setting.ID = newSetting.ID;
			setting.OwnerID = newSetting.OwnerID;

			return Ok(setting);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteSetting(int ID)
		{
			Setting existingSetting = _nest.Settings.All().Where(s => s.ID == ID).FirstOrDefault();

			if (existingSetting == null)
			{
				return BadRequest("No setting with the specified ID exists");
			}

			_nest.Settings.Delete(existingSetting);

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
		public IHttpActionResult GetSettingByID(int ID)
		{
			SettingModel setting = _nest.Settings.All().Where(s => s.ID == ID).Select(BuildSettingModel).FirstOrDefault();

			if (setting == null)
			{
				return BadRequest("No setting with the specified ID exists.");
			}

			return Ok(setting);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetSettingByOwnerIDAndType(int ownerID, string type)
		{
			SettingModel setting = _nest.Settings.All().Where(s => s.OwnerID == ownerID && s.OwnerType == type).Select(BuildSettingModel).FirstOrDefault();

			if (setting == null)
			{
				return BadRequest("No setting with the specified owner ID and type exists.");
			}

			return Ok(setting);
		}
    }
}
