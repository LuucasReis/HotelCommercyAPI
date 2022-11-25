using AutoMapper;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HotelComercy_WebAPI.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VillaApiController> _logger;
        private readonly IMapper _mapper;

        public VillaApiController(IUnitOfWork unitOfWork, ILogger<VillaApiController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Retornando todas as vilas.");
            var villas = await _unitOfWork.Vila.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villas));
        }

        [HttpGet("{id:int}", Name ="GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetById(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Erro em retornar a vila de id: " + id);
                return BadRequest();
            }
            var villa = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id, tracked: false);

            if (villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if(await _unitOfWork.Vila.GetFirstOrDefaultAsync(x=> x.Name == villaDTO.Name) != null)
            {
                ModelState.AddModelError("MatchingError", "Esse nome já existe!!");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            var villa = _mapper.Map<Villa>(villaDTO);

            await _unitOfWork.Vila.AddAsync(villa);
            await _unitOfWork.SaveAsync();
            return CreatedAtRoute("GetById", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            _unitOfWork.Vila.Remove(villa);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id , [FromBody] VillaUpdateDTO villaDTO)
        {
            if(villaDTO == null || villaDTO.Id != id)
            {
                return BadRequest();
            }
            var villa = _mapper.Map<Villa>(villaDTO);
            
            _unitOfWork.Vila.Update(villa);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> doc)
        {
            if(id == 0 || doc == null)
            {
                return BadRequest();
            }
            var villaFromDB = await _unitOfWork.Vila.GetFirstOrDefaultAsync(x => x.Id == id, tracked: false);
            
            VillaUpdateDTO  villaDTO = _mapper.Map<VillaUpdateDTO>(villaFromDB);

            doc.ApplyTo(villaDTO, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var villa = _mapper.Map<Villa>(villaDTO);

            _unitOfWork.Vila.Update(villa);
            await _unitOfWork.SaveAsync();
            return NoContent();

        }
    }
}
