using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_Utility;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _service;
        private readonly IMapper _mapper;

        public VillaController(IVillaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villa = new List<VillaDTO>();
            var response = await _service.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSucess)
            {
                villa = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(villa);
        }
        public IActionResult CreateVilla()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villaCreate)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.CreateAsync<ApiResponse>(villaCreate, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = "Villa criada com sucesso!!";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Erro ao criar villa";
            return View(villaCreate);
        }

        public async Task<IActionResult> UpdateVilla(int id)
        {

            var model = await _service.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (model != null && model.IsSucess)
            {
                var obj = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(model.Result));
                return View(_mapper.Map<VillaUpdateDTO>(obj));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdate)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync<ApiResponse>(villaUpdate, HttpContext.Session.GetString(SD.SessionToken));
                TempData["success"] = "Villa atualizada com sucesso!!";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Erro ao atualizar villa";

            return View(villaUpdate);
        }

        public async Task<IActionResult> DeleteVilla(int id)
        {
            var obj = await _service.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (obj != null && obj.IsSucess)
            {
                var model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(obj.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villa)
        {
            
            await _service.DeleteAsync<ApiResponse>(villa.Id, HttpContext.Session.GetString(SD.SessionToken));
            TempData["success"] = "Villa deletada com sucesso!!";
            return RedirectToAction(nameof(IndexVilla));
        }
    }
}
