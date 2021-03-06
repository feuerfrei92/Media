﻿using Data.Repositories;
using Models;
using Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class RepositoryNest : IRepositoryNest
	{
		private IContext _context;
		private IDictionary<Type, object> _repositories;

		public RepositoryNest() :
			this(new DBContext())
		{

		}

		public RepositoryNest(IContext context)
		{
			_context = context;
			_repositories = new Dictionary<Type, object>();
		}

		public IRepository<User> Users
		{
			get { return GetRepository<User>();  }
		}

		public IRepository<Profile> Profiles
		{
			get { return GetRepository<Profile>(); }
		}

		public IRepository<Section> Sections
		{
			get { return GetRepository<Section>(); }
		}

		public IRepository<Topic> Topics
		{
			get { return GetRepository<Topic>(); }
		}

		public IRepository<Comment> Comments
		{
			get { return GetRepository<Comment>(); }
		}

		public IRepository<Friendship> Friendships
		{
			get { return GetRepository<Friendship>(); }
		}

		public IRepository<Interest> Interests
		{
			get { return GetRepository<Interest>(); }
		}

		public IRepository<Follower> Followers
		{
			get { return GetRepository<Follower>(); }
		}

		public IRepository<Membership> Memberships
		{
			get { return GetRepository<Membership>(); }
		}

		public IRepository<Photo> Photos
		{
			get { return GetRepository<Photo>(); }
		}

		public IRepository<Video> Videos
		{
			get { return GetRepository<Video>(); }
		}

		public IRepository<Visit> Visits
		{
			get { return GetRepository<Visit>(); }
		}

		public IRepository<Album> Albums
		{
			get { return GetRepository<Album>(); }
		}

		public IRepository<Setting> Settings
		{
			get { return GetRepository<Setting>(); }
		}

		public IRepository<Message> Messages
		{
			get { return GetRepository<Message>(); }
		}

		public IRepository<Discussion> Discussions
		{
			get { return GetRepository<Discussion>(); }
		}

		public IRepository<Discussionist> Discussionists
		{
			get { return GetRepository<Discussionist>(); }
		}

		public IRepository<Vote> Votes
		{
			get { return GetRepository<Vote>(); }
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}

		private IRepository<T> GetRepository<T>() where T : class
		{
			var typeOfModel = typeof(T);
			if(!(_repositories.ContainsKey(typeOfModel)))
			{
				var type = typeof(Repository<T>);
				_repositories.Add(typeOfModel, Activator.CreateInstance(type, _context));
			}

			return (IRepository<T>)_repositories[typeOfModel];
		}
	}
}
