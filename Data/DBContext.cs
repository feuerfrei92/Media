using Data.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
	public class DBContext : IdentityDbContext<ApplicationUser>, IContext
	{
		public DBContext() :
			base("MediaConnectionString")
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Configuration>());
		}

		public virtual IDbSet<User> Users { get; set; }
		public virtual IDbSet<Profile> Profiles { get; set; }
		public virtual IDbSet<Section> Sections { get; set; }
		public virtual IDbSet<Topic> Topics { get; set; }
		public virtual IDbSet<Comment> Comments { get; set; }
		public virtual IDbSet<Friendship> Friendships { get; set; }
		public virtual IDbSet<Interest> Interests { get; set; }
		public virtual IDbSet<Follower> Followers { get; set; }
		public virtual IDbSet<Membership> Memberships { get; set; }
<<<<<<< HEAD
=======
		public virtual IDbSet<Photo> Photos { get; set; }
		public virtual IDbSet<Video> Videos { get; set; }
>>>>>>> 4b1decff8efb38854e3b29388171c1b61438b3b1

		public new void SaveChanges()
		{
			base.SaveChanges();
		}

		public static DBContext Create()
		{
			return new DBContext();
		}
	}
}
