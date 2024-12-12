using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory
{
	public class VillaReposaitory : Reposaitory<Villa>, IVillaReposaitory
	{
		private readonly ApplicationDbContext _db;

		public VillaReposaitory(ApplicationDbContext db):base(db) 
        {
			_db = db;
		}
      

		public async Task<Villa> UpdateAsync(Villa entity)
		{
			entity.UpdatedDate = DateTime.Now;
			_db.Villas.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
