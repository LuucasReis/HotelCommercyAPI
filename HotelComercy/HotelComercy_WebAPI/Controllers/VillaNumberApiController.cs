using AutoMapper;
using Azure;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelComercy_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberApiController : ControllerBase
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VillaNumberApiController> _logger;
        protected ApiResponse _apiResponse;

        public VillaNumberApiController(IUnitOfWork unityOfWork, IMapper mapper, ILogger<VillaNumberApiController> logger)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            try
            {
                _logger.LogInformation("Retornando todas as VilasNumber.");
                var villasNumber = await _unityOfWork.VillaNumber.GetAllAsync();
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<List<VillaNumberDTO>>(villasNumber);
                return Ok(_apiResponse);
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpGet("{id:int}", Name = "GetByIdVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Requisição de VillaNumber por id 0");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villaNumber = await _unityOfWork.VillaNumber.GetFirstOrDefaultAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                _unityOfWork.VillaNumber.Update(villaNumber);
                await _unityOfWork.SaveAsync();

                var villaNumberDTO = _mapper.Map<VillaNumberDTO>(villaNumber);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = villaNumberDTO;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] VillaNumberCreateDTO villaNumber)
        {
            try
            {
                if (await _unityOfWork.VillaNumber.GetFirstOrDefaultAsync(x => x.VillaNo == villaNumber.VillaNo) != null)
                {
                    ModelState.AddModelError("villaNumberErro", "Número da villa já existe!!");
                    return BadRequest(ModelState);
                }

                if (await _unityOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == villaNumber.VillaId) == null)
                {
                    ModelState.AddModelError("villaNumberErro", "Número da villaId não existe!!");
                    return BadRequest(ModelState);
                }

                if (villaNumber == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = _mapper.Map<VillaNumber>(villaNumber);

                await _unityOfWork.VillaNumber.AddAsync(villa);
                await _unityOfWork.SaveAsync();

                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = _mapper.Map<VillaNumberDTO>(villa);

                return CreatedAtRoute("GetByIdVillaNumber", new { id = villa.VillaNo }, _apiResponse);
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
        public async Task<ActionResult<ApiResponse>> Put(int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdate)
        {
            try
            {
                if (id == 0 || id != villaNumberUpdate.VillaNo)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                if (await _unityOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == villaNumberUpdate.VillaId) == null)
                {
                    ModelState.AddModelError("villaNumberErro", "Número da villaId não existe!!");
                    return BadRequest(ModelState);
                }

                var villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdate);
                _unityOfWork.VillaNumber.Update(villaNumber);
                await _unityOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                var villa = await _unityOfWork.VillaNumber.GetFirstOrDefaultAsync(x => x.VillaNo == id);
                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                _unityOfWork.VillaNumber.Remove(villa);
                await _unityOfWork.SaveAsync();

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Patch(int id, JsonPatchDocument<VillaNumberUpdateDTO> villaNumberUpdate)
        {
            try
            {
                if (id == 0 || villaNumberUpdate == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = await _unityOfWork.VillaNumber.GetFirstOrDefaultAsync(x => x.VillaNo == id, tracked: false);
                var villaNumber = _mapper.Map<VillaNumberUpdateDTO>(villa);

                villaNumberUpdate.ApplyTo(villaNumber);
                if (!ModelState.IsValid)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villaNumberDb = _mapper.Map<VillaNumber>(villaNumber);
                _unityOfWork.VillaNumber.Update(villaNumberDb);
                await _unityOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages = new List<string> { e.ToString() };
            }
            return _apiResponse;
        }
    }
}
