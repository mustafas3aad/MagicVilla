using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory.IReposaitory
{
	public interface IReposaitory<T>  where T :class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeproperties = null,
			
			int pageSize = 0, int pageNumber = 1);

		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeproperties = null);
		
		Task CreateAsync(T entity);
		Task RemoveAsync(T entity);
		Task SaveAsync();
	}
}
