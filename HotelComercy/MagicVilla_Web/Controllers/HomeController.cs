using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Villa_Utility;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper; 
        public HomeController(IVillaService service,IMapper mapper)
        {
            _villaService = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villas = new();

            var obj = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if(obj != null && obj.IsSucess)
            {
                villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(obj.Result));
            }
            return View(villas);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}