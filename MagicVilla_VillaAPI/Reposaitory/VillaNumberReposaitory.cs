using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory
{
	public class VillaNumberReposaitory : Reposaitory<VillaNumber>, IVillaNumberReposaitory
	{
		private readonly ApplicationDbContext _db;

		public VillaNumberReposaitory(ApplicationDbContext db):base(db) 
        {
			_db = db;
		}
      

		public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
		{
			entity.UpdatedDate = DateTime.Now;
			_db.VillaNumbers.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}
