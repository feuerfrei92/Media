using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
	public interface IRepository<T> where T : class
	{
		IDbSet<T> All();
		void Create(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Detach(T entity);
		void SaveChanges();
	}
}
