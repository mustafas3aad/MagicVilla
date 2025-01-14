﻿using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.Iservices
{
	public interface IVillaNumberService
	{
		Task<T> GetAllAsync<T>(string token);
		Task<T> GetAsync<T>(int id, string token);
		Task<T> CreateAsync<T>(VillaNumberCreateDTO dTO, string token);
		Task<T> DeleteAsync<T>(int id, string token);
		Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dTO, string token);
		
	}
}
