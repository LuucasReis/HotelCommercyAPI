using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelComercy_WebAPI.Controllers
{
    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
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
                return BadRequest();
            }
            var villa = _unitOfWork.Vila.GetFirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            return Ok(new VillaDTO(villa.Id, villa.Name));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] Villa villa)
        {
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
            var villaDTO = new VillaDTO(villa.Id, villa.Name);
            return CreatedAtRoute("GetById", new { id = villaDTO.id }, villaDTO);
        }
    }
}
