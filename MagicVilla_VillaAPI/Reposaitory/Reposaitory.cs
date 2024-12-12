using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory
{
	public class Reposaitory<T>:IReposaitory<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T > _dbSet;

		public Reposaitory(ApplicationDbContext db)
		{
			_db = db;
			
			_dbSet = _db.Set<T>();
		}
		public async Task CreateAsync(T entity)
		{
			await _db.AddAsync(entity);
			await SaveAsync();
		}

		
		public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeproperties = null)
		{

			IQueryable<T> query = _dbSet;
			if (!tracked)
			{
				query = query.AsNoTracking();
			}
			if (filter != null)
			{
				query = query.Where(filter);
			}
         
			if (includeproperties != null)
			{
				foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeprop);
				}
			}
			return await query.FirstOrDefaultAsync();
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeproperties = null,
            int pageSize = 0, int pageNumber = 1)
		{
			IQueryable<T> query = _dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}


            
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }


            if (includeproperties != null)
			{
				foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeprop);
				}
			}
			return await query.ToListAsync();
		}

		public async Task RemoveAsync(T entity)
		{
			_dbSet.Remove(entity);
			await SaveAsync();
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}

	
	}
}
