using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
	
    [Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	
	[ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
	{
		
		protected APIResponse _response;
		private readonly IVillaNumberReposaitory _dbVillaNumber;

		public IVillaReposaitory _dbVilla { get; }


		public IMapper _mapper { get; }

		

		public VillaNumberAPIController( IVillaNumberReposaitory dbVillaNumber,IVillaReposaitory dbVilla,IMapper mapper)
        {
			_dbVillaNumber = dbVillaNumber;
			_dbVilla = dbVilla;
			_mapper = mapper;
			this._response = new ();
		}




		
		[HttpGet("GetString")]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}
	}
}
