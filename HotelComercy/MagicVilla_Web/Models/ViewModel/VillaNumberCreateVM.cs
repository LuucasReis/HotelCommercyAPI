using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
    public class VillaNumberCreateVM
    {
        public VillaNumberCreateDTO VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Villas { get; set; }
    }
}
