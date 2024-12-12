using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Reposaitory.IReposaitory
{
	public interface IVillaNumberReposaitory:IReposaitory<VillaNumber>
	{
		

		Task<VillaNumber> UpdateAsync(VillaNumber entity);

		
	}
}
