using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Iservices;

namespace MagicVilla_Web.Services
{
	public class VillaNumberService : BaseService, IVillaNumberService
    {
		private readonly IHttpClientFactory _clientFactory;
		private string villaUrl;

		public VillaNumberService(IHttpClientFactory clientFactory,IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;

			villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
		}

		public Task<T> CreateAsync<T>(VillaNumberCreateDTO dTO, string token)
		{
		
			return SendAsync<T>(new APIRequest()
			{
				ApiType=SD.ApiType.POST,
				Data = dTO,
		
				Url = villaUrl + "/api/v1/villaNumberAPI",
				Token = token

            });
		}

		public Task<T> DeleteAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = villaUrl + "/api/v1/villaNumberAPI/" + id,
				Token = token

			});
		}

		public Task<T> GetAllAsync<T>(string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaUrl + "/api/v1/villaNumberAPI",
				Token = token

            });
		}

		public Task<T> GetAsync<T>(int id, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = villaUrl + "/api/v1/villaNumberAPI/" + id,
				Token = token

			});
		}

		public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dTO, string token)
		{
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.PUT,
				Data = dTO,

				Url = villaUrl + "/api/v1/villaNumberAPI/" + dTO.VillaNo,
				Token = token

			});
		}
	}
}
