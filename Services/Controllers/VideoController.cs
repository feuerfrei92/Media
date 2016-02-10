﻿using Data;
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
    public class VideoController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Video, VideoModel>> BuildVideoModel
		{
			get { return v => new VideoModel { ID = v.ID, Location = v.Location, DateCreated = v.DateCreated, OwnerID = v.OwnerID }; }
		}

		public VideoController()
			: this(new RepositoryNest())
		{

		}

		public VideoController(RepositoryNest nest)
		{
			_nest = nest;
		}

		[HttpGet]
		public IHttpActionResult GetAllVideos()
		{
			IQueryable<VideoModel> videos = _nest.Videos.All().Select(BuildVideoModel);
			return Ok(videos);
		}

		[HttpPost]
		public IHttpActionResult CreateVideo(int userID, VideoModel video)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			var newVideo = new Video
			{
				Location = video.Location,
				OwnerID = userID,
				DateCreated = DateTime.Now
			};

			_nest.Videos.Create(newVideo);

			try
			{
				_nest.SaveChanges();
			}
			catch
			{
				throw;
			}

			video.ID = newVideo.ID;

			return Ok(video);
		}

		[HttpDelete]
		public IHttpActionResult DeleteVideo(int ID)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			Video existingVideo = _nest.Videos.All().Where(v => v.ID == ID).FirstOrDefault();

			if (existingVideo == null)
			{
				return BadRequest("No video with the specified ID exists.");
			}

			_nest.Videos.Delete(existingVideo);

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
		public IHttpActionResult GetVideoByID(int ID)
		{
			VideoModel video = _nest.Videos.All().Where(v => v.ID == ID).Select(BuildVideoModel).FirstOrDefault();

			if (video == null)
			{
				return BadRequest("No video with the specified ID exists.");
			}

			return Ok(video);
		}

		[HttpGet]
		public IHttpActionResult GetVideosForOwner(int ownerID)
		{
			List<VideoModel> videos = _nest.Videos.All().Where(v => v.OwnerID == ownerID).Select(BuildVideoModel).ToList();

			return Ok(videos);
		}

		[HttpPut]
		public IHttpActionResult UpdateRating(int videoID, bool like)
		{
			Video video = _nest.Videos.All().Where(v => v.ID == videoID).FirstOrDefault();

			if (like)
				video.Rating++;
			else
				video.Rating--;

			_nest.Videos.Update(video);

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
