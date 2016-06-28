using Models;
using Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public interface IContext
	{
		IDbSet<User> Users { get; set; }
		IDbSet<Profile> Profiles { get; set; }
		IDbSet<Section> Sections { get; set; }
		IDbSet<Topic> Topics { get; set; }
		IDbSet<Comment> Comments { get; set; }
		IDbSet<Friendship> Friendships { get; set; }
		IDbSet<Interest> Interests { get; set; }
		IDbSet<Follower> Followers { get; set; }
		IDbSet<Membership> Memberships { get; set; }
		IDbSet<Photo> Photos { get; set; }
		IDbSet<Video> Videos { get; set; }
		IDbSet<Visit> Visits { get; set; }
		IDbSet<Album> Albums { get; set; }
		IDbSet<Setting> Settings { get; set; }
		IDbSet<Message> Messages { get; set; }
		IDbSet<Discussion> Discussions { get; set; }
		IDbSet<Discussionist> Discussionists { get; set; }
		IDbSet<Vote> Votes { get; set; }
		void SaveChanges();
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
	}
}
