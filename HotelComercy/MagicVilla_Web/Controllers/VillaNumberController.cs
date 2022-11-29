using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _service;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> villas = new();

            var obj = await _service.GetAllAsync<ApiResponse>();
            if (obj != null && obj.IsSucess)
            {
                villas = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(obj.Result));
            }
            return View(villas);

        }
    }
}
