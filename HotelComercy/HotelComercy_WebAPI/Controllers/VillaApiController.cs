using HotelComercy_WebAPI.Logging;
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

        public VillaApiController(IUnitOfWork unitOfWork, ILogger<VillaApiController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Retornando todas as vilas.");
            var villas = _unitOfWork.Vila.GetAll();
            return Ok(villas);
        }

        [HttpGet("{id:int}", Name ="GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetById(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Erro em retornar a vila de id: " + id);
                return BadRequest();
            }
            var villa = _unitOfWork.Vila.GetFirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            return Ok(new VillaDTO(villa.Id, villa.Name, villa.Occupancy, villa.Sqtf));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] Villa villa)
        {
            if(_unitOfWork.Vila.GetFirstOrDefault(x=> x.Name == villa.Name) != null)
            {
                ModelState.AddModelError("MatchingError", "Esse nome já existe!!");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest();
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            _unitOfWork.Vila.Add(villa);
            _unitOfWork.Save();
            var villaDTO = new VillaDTO(villa.Id, villa.Name, villa.Occupancy, villa.Sqtf);
            return CreatedAtRoute("GetById", new { id = villaDTO.id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _unitOfWork.Vila.GetFirstOrDefault(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            _unitOfWork.Vila.Remove(villa);
            _unitOfWork.Save();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id , [FromBody] Villa villa)
        {
            if(villa == null || villa.Id != id)
            {
                return BadRequest();
            }
            var villaFromDB = _unitOfWork.Vila.GetFirstOrDefault(x => x.Id == id);
            
            villaFromDB.Name = villa.Name;
            villaFromDB.Occupancy = villa.Occupancy;
            villaFromDB.Sqtf = villa.Sqtf;
            _unitOfWork.Vila.Update(villaFromDB);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePatchVilla(int id, JsonPatchDocument<Villa> doc)
        {
            if(id == 0 || doc == null)
            {
                return BadRequest();
            }
            var villaFromDB = _unitOfWork.Vila.GetFirstOrDefault(x => x.Id == id);
            if(villaFromDB == null)
            {
                return NotFound();
            }

            doc.ApplyTo(villaFromDB);
            _unitOfWork.Save();
            return NoContent();

        }
    }
}
