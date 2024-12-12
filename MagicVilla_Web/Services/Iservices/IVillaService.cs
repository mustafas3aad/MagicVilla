using Humanizer;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Hosting.Server;
using NuGet.Common;
using NuGet.Configuration;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;
namespace MagicVilla_Web.Services.Iservices
{
    public interface IVillaService
    {




        Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(VillaCreateDTO dTO, string token);
		Task<T> DeleteAsync<T>(int id, string token);
		Task<T> UpdateAsync<T>(VillaUpdateDTO dTO, string token);
		
	}
}
