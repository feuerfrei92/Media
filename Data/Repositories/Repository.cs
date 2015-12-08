using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private IContext _context;
		private IDbSet<T> _set;

		public Repository() :
			this(new DBContext())
		{

		}

		public Repository(IContext context)
		{
			_context = context;
			_set = context.Set<T>();
		}

		public IDbSet<T> All()
		{
			return _set;
		}

		public void Create(T entity)
		{
			ChangeState(entity, EntityState.Added);
		}

		public void Update(T entity)
		{
			ChangeState(entity, EntityState.Modified);
		}

		public void Delete(T entity)
		{
			ChangeState(entity, EntityState.Deleted);
		}

		public void Detach(T entity)
		{
			ChangeState(entity, EntityState.Detached);
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}

		private void ChangeState(T entity, EntityState state)
		{
			var entry = _context.Entry<T>(entity);
			if (entry.State == EntityState.Detached)
				_set.Attach(entity);

			entry.State = state;
		}
	}
}
