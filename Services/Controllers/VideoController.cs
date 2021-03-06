﻿using Data;
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
    public class VideoController : ApiController
    {
		private IRepositoryNest _nest;

		private static Expression<Func<global::Models.DatabaseModels.Video, VideoModel>> BuildVideoModel
		{
			get { return v => new VideoModel { ID = v.ID, Location = v.Location, DateCreated = v.DateCreated, OwnerID = v.OwnerID, IsProfile = v.IsProfile, IsInterest = v.IsInterest }; }
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
		[Authorize]
		public IHttpActionResult GetAllVideos()
		{
			IQueryable<VideoModel> videos = _nest.Videos.All().Select(BuildVideoModel);
			return Ok(videos);
		}

		[HttpPost]
		[Authorize]
		public IHttpActionResult CreateVideo(int userID, VideoModel video)
		{
			if (!(ModelState.IsValid))
			{
				return BadRequest(ModelState);
			}

			try
			{
				var newVideo = new Video
				{
					Location = video.Location,
					OwnerID = userID,
					DateCreated = DateTime.Now,
					IsInterest = video.IsInterest,
					IsProfile = video.IsProfile,
					Rating = 0,
				};

				_nest.Videos.Create(newVideo);

				var newTopic = new Topic
				{
					Name = string.Concat(video.DateCreated.ToLongDateString(), video.DateCreated.ToLongTimeString()),
					SectionID = video.ID,
					AuthorID = video.OwnerID,
					DateCreated = DateTime.Now,
					TopicType = "Video",
				};

				_nest.Topics.Create(newTopic);

				var newSetting = new Setting
				{
					OwnerID = video.ID,
					OwnerType = "Video",
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

				video.ID = newVideo.ID;

				return Ok(video);
			}
			catch
			{
				throw;
			}
		}

		[HttpDelete]
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		public IHttpActionResult GetVideosForOwner(int userID)
		{
			List<VideoModel> videos = _nest.Videos.All().Where(v => v.OwnerID == userID && v.IsProfile).Select(BuildVideoModel).ToList();

			return Ok(videos);
		}

		[HttpGet]
		[Authorize]
		public IHttpActionResult GetVideosForInterest(int interestID)
		{
			List<VideoModel> videos = _nest.Videos.All().Where(v => v.OwnerID == interestID && v.IsInterest).Select(BuildVideoModel).ToList();

			return Ok(videos);
		}

		[HttpPut]
		[Authorize]
		public IHttpActionResult UpdateRating(int videoID, int voterID, bool like)
		{
			Video video = _nest.Videos.All().Where(v => v.ID == videoID).FirstOrDefault();
			Vote existingVote = _nest.Votes.All().Where(v => v.TargetID == videoID && v.Type == "Video" && v.VoterID == voterID).FirstOrDefault();

            if (existingVote != null && existingVote.IsLike == like)
                return BadRequest("You cannot vote twice the same");

            if (existingVote != null && existingVote.IsLike != like && like)
                video.Rating += 2;

            if (existingVote != null && existingVote.IsLike != like && !like)
                video.Rating -= 2;

            if (existingVote == null && like)
                video.Rating++;
            if (existingVote == null && !like)
                video.Rating--;

			_nest.Videos.Update(video);

			var vote = new Vote
			{
				TargetID = videoID,
				VoterID = voterID,
				Type = "Video",
			};

			_nest.Votes.Create(vote);

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
