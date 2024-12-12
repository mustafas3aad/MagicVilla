using MagicVilla_Web.Models;
using APIResponse = MagicVilla_Web.Models.APIResponse;

namespace MagicVilla_Web.Services.Iservices
{
	public interface IBaseService
	{
		
		APIResponse responseModel { get; set; }
		
		Task<T> SendAsync<T>(APIRequest apiRequest);



	}
}
