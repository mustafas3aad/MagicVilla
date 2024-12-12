using AutoMapper;
using Azure.Core;
using Azure;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Reposaitory.IReposaitory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("1.0")]
   
    [ApiController]

    public class VillaAPIController : ControllerBase
    {
        
        protected APIResponse _response;
        private readonly IVillaReposaitory _dbVilla;

        

        public IMapper _mapper { get; }

        

        public VillaAPIController(/*ILogger<VillaAPIController> logger*/ /*ILogging logger*/ IVillaReposaitory dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;

            
            _mapper = mapper;
            //_logger = logger;
            //_logger = logger;

            _response = new();
        }



        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        
        public async Task<ActionResult< APIResponse>> GetVillas([FromQuery(Name ="filterOccupancy")]int? occupancy,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {

            //_logger.LogInformation("Getting all villas");

            //_logger.Log("Getting all villas","");
            try
            {

                
                IEnumerable<Villa> Villalist;
                if(occupancy > 0)
                {
                    Villalist = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize,
                        pageNumber:pageNumber);
                        
                }
                else
                {
                    Villalist = await _dbVilla.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }
           

                if (!string.IsNullOrEmpty(search))
                {
                    Villalist=Villalist.Where(u=>u.Name.ToLower().Contains(search));
                } 

               
                Pagination pagination = new() { PageNumber=pageNumber, PageSize=pageSize};
                Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));


                _response.Result = _mapper.Map<List<VillaDTO>>(Villalist);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
                

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

       



       

          
        public async Task<ActionResult< APIResponse>> GetVilla(int id)
        {

            //_logger.Log("Get Villa Error with Id" + id,"error");
            //_logger.LogError("Get Villa Error with Id" + id);
            try
            {
                if (id == 0)
                {
                    
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess=false;
                    return BadRequest(_response);
                    

                }
                
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                    
                }

                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult< APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO )
        {
          

            try
            {

                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already Exists");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

               
                Villa villa = _mapper.Map<Villa>(createDTO);

              

                

                await _dbVilla.CreateAsync(villa);

              

                _response.Result = _mapper.Map<Villa>(createDTO);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;



        }



        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {


                if (id == 0)
                {
                    return BadRequest();
                }
                
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                

                await _dbVilla.RemoveAsync(villa);


                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdataVilla")]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, VillaUpdateDTO updateDTO)
        {
            try
            {


                if (id != updateDTO.Id || updateDTO == null)
                {
                    return BadRequest();
                }
                

                Villa model = _mapper.Map<Villa>(updateDTO);

              
                await _dbVilla.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;
        }

        
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVilla(int id, JsonPatchDocument< VillaUpdateDTO> patchDto)
        {
            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }
            
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

        

            if (villa == null)
            {
                return BadRequest();
            }


            patchDto.ApplyTo(villaDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaDTO);

         
            await _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
