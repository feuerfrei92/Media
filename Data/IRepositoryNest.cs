using Data.Repositories;
using Models;
using Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public interface IRepositoryNest
	{
		IRepository<User> Users { get; }
		IRepository<Profile> Profiles { get; }
		IRepository<Section> Sections { get; }
		IRepository<Topic> Topics { get; }
		IRepository<Comment> Comments { get; }
		IRepository<Friendship> Friendships { get; }
		IRepository<Interest> Interests { get; }
		IRepository<Follower> Followers { get; }
		IRepository<Membership> Memberships { get; }
		IRepository<Photo> Photos { get; }
		IRepository<Video> Videos { get; }
		void SaveChanges();
	}
}
