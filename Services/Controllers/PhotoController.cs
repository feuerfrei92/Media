using Data;
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
    public class PhotoController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Photo, PhotoModel>> BuildPhotoModel
		{
			get { return p => new PhotoModel { ID = p.ID, Content = p.Content, DateCreated = p.DateCreated, OwnerID = p.OwnerID }; }
		}

		public PhotoController()
			: this(new RepositoryNest())
		{

		}

		public PhotoController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		public IHttpActionResult GetAllPhotos()
		{
			IQueryable<PhotoModel> photos = _nest.Photos.All().Select(BuildPhotoModel);
			return Ok(photos);
		}

		[HttpPost]
		public IHttpActionResult CreatePhoto(int userID, PhotoModel photo)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newPhoto = new Photo
			{
				Content = photo.Content,
				OwnerID = userID,
				DateCreated = DateTime.Now
			};

			_nest.Photos.Create(newPhoto);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			photo.ID = newPhoto.ID;

			return Ok(photo);
		}

		[HttpDelete]
		public IHttpActionResult DeletePhoto(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Photo existingPhoto = _nest.Photos.All().Where(p => p.ID == ID).FirstOrDefault();

			if (existingPhoto == null)
			{
				return BadRequest("No photo with the specified ID exists.");
			}

			_nest.Photos.Delete(existingPhoto);

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
		public IHttpActionResult GetPhotoByID(int ID)
		{
			PhotoModel photo = _nest.Photos.All().Where(p => p.ID == ID).Select(BuildPhotoModel).FirstOrDefault();

			if (photo == null)
			{
				return BadRequest("No photo with the specified ID exists.");
			}

			return Ok(photo);
		}

		[HttpGet]
		public IHttpActionResult GetPhotosForOwner(int ownerID)
		{
			List<PhotoModel> photos = _nest.Photos.All().Where(p => p.OwnerID == ownerID).Select(BuildPhotoModel).ToList();

			return Ok(photos);
		}

		[HttpPut]
		public IHttpActionResult UpdateRating(int photoID, bool like)
		{
			Photo photo = _nest.Photos.All().Where(p => p.ID == photoID).FirstOrDefault();

			if (like)
				photo.Rating++;
			else
				photo.Rating--;

			_nest.Photos.Update(photo);

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
    }
}
