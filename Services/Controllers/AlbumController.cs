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
    public class AlbumController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Album, AlbumModel>> BuildAlbumModel
		{
			get { return a => new AlbumModel { ID = a.ID, Name = a.Name, OwnerID = a.OwnerID, IsInterest = a.IsInterest, IsProfile = a.IsProfile, Size = a.Size, Rating = a.Rating }; }
		}

		private static Expression<Func<global::Models.DatabaseModels.Photo, PhotoModel>> BuildPhotoModel
		{
			get { return p => new PhotoModel { ID = p.ID, Content = p.Content, DateCreated = p.DateCreated, AlbumID = p.AlbumID }; }
		}

		public AlbumController()
			: this(new RepositoryNest())
		{

		}

		public AlbumController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAllAlbums()
		{
			IQueryable<AlbumModel> albums = _nest.Albums.All().Select(BuildAlbumModel);
			return Ok(albums);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAllPhotos()
		{
			IQueryable<PhotoModel> photos = _nest.Photos.All().Select(BuildPhotoModel);
			return Ok(photos);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateAlbum(int ownerID, AlbumModel album)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newAlbum = new Album
			{
				Name = album.Name,
				OwnerID = ownerID,
				IsInterest = album.IsInterest,
				IsProfile = album.IsProfile,
				Size = 0,
				Rating = 0,
			};

			_nest.Albums.Create(newAlbum);

			var newTopic = new Topic
			{
				Name = album.Name,
				SectionID = newAlbum.ID,
				AuthorID = ownerID,
				DateCreated = DateTime.Now,
				IsProfileTopic = false,
				IsInterestTopic = false,
			};

			_nest.Topics.Create(newTopic);

			var newSetting = new Setting
			{
				OwnerID = newAlbum.ID,
				OwnerType = "Album",
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

			album.ID = newAlbum.ID;

			return Ok(album);
		}

		[HttpDelete]
		[Authorize]
		public IHttpActionResult DeleteAlbum(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Album existingAlbum = _nest.Albums.All().Where(a => a.ID == ID).FirstOrDefault();

			if (existingAlbum == null)
			{
				return BadRequest("No album with the specified ID exists.");
			}

			_nest.Albums.Delete(existingAlbum);

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
		public IHttpActionResult GetAlbumByID(int ID)
		{
			AlbumModel album = _nest.Albums.All().Where(a => a.ID == ID).Select(BuildAlbumModel).FirstOrDefault();

			if (album == null)
			{
				return BadRequest("No album with the specified ID exists.");
			}

			return Ok(album);
		}

		[HttpGet]
		public IHttpActionResult GetAlbumsForProfile(int ownerID)
		{
			List<AlbumModel> albums = _nest.Albums.All().Where(a => a.OwnerID == ownerID && a.IsProfile == true).Select(BuildAlbumModel).ToList();

			return Ok(albums);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetAlbumsForInterest(int ownerID)
		{
			List<AlbumModel> albums = _nest.Albums.All().Where(a => a.OwnerID == ownerID && a.IsInterest == true).Select(BuildAlbumModel).ToList();

			return Ok(albums);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateAlbumRating(int albumID, bool like)
		{
			Album album = _nest.Albums.All().Where(a => a.ID == albumID).FirstOrDefault();

			if (like)
				album.Rating++;
			else
				album.Rating--;

			_nest.Albums.Update(album);

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
		public IHttpActionResult CreatePhoto(int albumID, PhotoModel photo)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newPhoto = new Photo
			{
				Content = photo.Content,
				AlbumID = albumID,
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public IHttpActionResult GetPhotosForAlbum(int albumID)
		{
			List<PhotoModel> photos = _nest.Photos.All().Where(p => p.AlbumID == albumID).Select(BuildPhotoModel).ToList();

			return Ok(photos);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdatePhotoRating(int photoID, bool like)
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
