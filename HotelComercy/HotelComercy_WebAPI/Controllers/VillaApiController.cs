using AutoMapper;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Pagination;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.Net;
using System.Text.Json;

namespace HotelComercy_WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaApi")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaApiController : ControllerBase
    {
        protected ApiResponse _apiResponse;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VillaApiController> _logger;
        private readonly IMapper _mapper;

        public VillaApiController(IUnitOfWork unitOfWork, ILogger<VillaApiController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _apiResponse = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Padrao30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> GetVillas([FromQuery] int? Occupancy, [FromQuery]string? name, [FromQuery]int? pageNumber
            ,[FromQuery]int? pageSize)
        {
            try
            {
                IEnumerable<Villa> villas;
                DefaultPagination defaultPagination = new();
                if (pageNumber != null && pageSize != null)
                {
                    defaultPagination.PageNumber = pageNumber.Value;
                    defaultPagination.PageSize = pageSize.Value;
                }
                if (Occupancy > 0)
                {
                    _logger.LogInformation("Retornando vilas com base em um filtro.");
                    villas = await _unitOfWork.Vila.GetAllAsync(x => x.Occupancy == Occupancy, pagination: defaultPagination);

                }
                else
                {
                    _logger.LogInformation("Retornando todas as vilas.");
                    villas = await _unitOfWork.Vila.GetAllAsync(pagination: defaultPagination);
                }
                
                if(!string.IsNullOrEmpty(name))
                {
                    villas = villas.Where(x=> x.Name.ToLower().Contains(name));
                }
                PagedInfo pagedInfo = new PagedInfo(defaultPagination.TotalPages, defaultPagination.PageNumber, defaultPagination.PageSize);

                var metadata = new
                {
                    pagedInfo.TotalCount,
                    defaultPagination.PageSize,
                    pagedInfo.CurrentPage,
                    pagedInfo.TotalPages,
                    pagedInfo.HasNext,
                    pagedInfo.HasPrevious
                };
                
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                _apiResponse.Result = _mapper.Map<List<VillaDTO>>(villas);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;

        }

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _logger.LogError("Erro em retornar a vila de id: " + id);
                    return BadRequest(_apiResponse);
                }
                var villa = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id, tracked: false);

                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;

                    return NotFound(_apiResponse);
                }
                _apiResponse.Result = _mapper.Map<VillaDTO>(villa);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            try
            {
                if (await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Name == villaDTO.Name) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Esse nome já existe!!");
                    return BadRequest(ModelState);
                }

                if (villaDTO == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                var villa = _mapper.Map<Villa>(villaDTO);

                await _unitOfWork.Vila.AddAsync(villa);
                await _unitOfWork.SaveAsync();

                _apiResponse.Result = _mapper.Map<VillaDTO>(villa);
                _apiResponse.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetById", new { id = villa.Id }, _apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id);
                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }
                _unitOfWork.Vila.Remove(villa);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null || villaDTO.Id != id)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                var villa = _mapper.Map<Villa>(villaDTO);

                _unitOfWork.Vila.Update(villa);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> doc)
        {
            if (id == 0 || doc == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            var villaFromDB = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id, tracked: false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villaFromDB);

            doc.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            var villa = _mapper.Map<Villa>(villaDTO);

            _unitOfWork.Vila.Update(villa);
            await _unitOfWork.SaveAsync();

            return NoContent();

        }
    }
}
