using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Interfaces
{
	public interface IRepository<T> where T : IEntity
	{
		//T - Category
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int offset = 0, int limit = 0);
		T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
		void Add(T entity);
		void Update(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
		void Save();
		bool IsStored(int id);
	}
}
