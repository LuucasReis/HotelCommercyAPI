using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.ViewModel;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Villa_Utility;

namespace MagicVilla_Web.Controllers
{
    [Authorize]
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _service;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService service, IMapper mapper, IVillaService villaService)
        {
            _service = service;
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> villas = new();

            var obj = await _service.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (obj != null && obj.IsSucess)
            {
                villas = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(obj.Result));
            }
            return View(villas);

        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaVM = new();
            var response = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null)
            {
                villaVM.Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }

            return View(villaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.CreateAsync<ApiResponse>(villaNumberVM.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = "VillaNumber criada com sucesso!!";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var respDropDown = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respDropDown != null)
            {
                villaNumberVM.Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respDropDown.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }
            TempData["error"] = "Erro ao criar VillaNumber";
            return View(villaNumberVM);
        }

        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            VillaNumberUpdateVM villaVM = new();
            var response = await _service.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSucess)
            {
                VillaNumberDTO obj = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(obj);
            }

            var respDropDown = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respDropDown != null)
            {
                villaVM.Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respDropDown.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
                return View(villaVM);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaUpdate)
        {

            if (ModelState.IsValid)
            {
                var response = await _service.UpdateAsync<ApiResponse>(villaUpdate.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                TempData["success"] = "VillaNumber atualizada com sucesso!!";
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            var respDropDown = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respDropDown != null)
            {
                villaUpdate.Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respDropDown.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }
            TempData["error"] = "Erro ao atualizar VillaNumber";
            return View(villaUpdate);
        }

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            VillaNumberDeleteVM deleteVM = new();
            var response = await _service.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if(response != null && response.IsSucess)
            {
                deleteVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            }

            var respDropDown = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respDropDown != null)
            {
                deleteVM.Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respDropDown.Result))
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    });
            }

            return View(deleteVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaVM)
        {
            var response = await _service.DeleteAsync<ApiResponse>(villaVM.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
            TempData["success"] = "VillaNumber deletada com sucesso!!";
            return RedirectToAction(nameof(IndexVillaNumber));
        }
    }
}
