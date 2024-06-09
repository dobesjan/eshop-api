using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models;
using Eshop.Api.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbset;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			this.dbset = _db.Set<T>();
		}

		public T Add(T entity, bool save = false)
		{
            dbset.Add(entity);

			if (save)
			{
				Save();
			}

			return entity;
		}

		public T Update(T entity, bool save = false)
		{
			_db.ChangeTracker.Clear();
			dbset.Update(entity);

			if (save)
			{
				Save();
			}

			return entity;
		}

		public void Detach(T entity)
		{
			var entry = dbset.Entry(entity);
			if (entry.State != EntityState.Detached)
			{
				entry.State = EntityState.Detached;
			}
		}

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = GetInternal(includeProperties, tracked);
			return query.Where(filter).FirstOrDefault();
		}

		public T Get(int id, string? includeProperties = null, bool tracked = false)
		{
			IQueryable<T> query = GetInternal(includeProperties, tracked);
			return query.Where(e => e.Id == id).FirstOrDefault();
		}

		private IQueryable<T> GetInternal(string? includeProperties, bool tracked)
		{
			IQueryable<T> query;

			if (tracked)
			{
				query = dbset;
			}
			else
			{
				//Db will not be automatically updated on entity change
				query = dbset.AsNoTrackingWithIdentityResolution();
			} 

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			return query;
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int offset = 0, int limit = 0)
		{
			IQueryable<T> query = dbset.AsNoTrackingWithIdentityResolution();
			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var includeProp in includeProperties
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}

			if (limit > 0)
			{
				query = query.Skip(offset).Take(limit);
			}

			return query.ToList();
		}

		public int Count(Expression<Func<T, bool>>? filter = null)
		{
			if (filter != null)
			{
				return dbset.Count(filter);
			}
			return dbset.Count();
		}

		public void Remove(T entity)
		{
			dbset.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			dbset.RemoveRange(entity);
		}

		public void Save()
		{
			_db.SaveChanges();
		}

		public bool IsStored(int id)
		{
			var storedEntity = Get(e => e.Id == id);
			if (storedEntity != null)
			{
				return true;
			}

			return false;
		}
	}
}
