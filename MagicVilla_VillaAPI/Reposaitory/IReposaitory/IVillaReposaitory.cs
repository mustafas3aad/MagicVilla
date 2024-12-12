using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory.IReposaitory
{
	public interface IVillaReposaitory:IReposaitory<Villa>
	{
		

		Task<Villa> UpdateAsync(Villa entity);

		
	}
}
