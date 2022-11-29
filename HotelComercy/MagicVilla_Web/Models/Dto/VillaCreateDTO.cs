using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaCreateDTO
    {
        
        [Required(ErrorMessage ="O campo {0} é obrigatório!!")]
        [MaxLength(30,ErrorMessage ="O campo {0} deve ter no max {1} caracteres.")]
        [Display(Name ="Nome")]
        public string Name { get; set; }
        [Display(Name = "Detalhes")]
        public string Details { get; set; }
        [Display(Name = "Ocupação")]
        public int Occupancy { get; set; }
        public int Sqtf { get; set; }
        [Display(Name = "Preço")]
        public double Rate { get; set; }
        [Display(Name = "Url Imagem")]
        public string ImageUrl { get; set; }
        [Display(Name = "Amenity")]
        public string Amenity { get; set; }
    }
}
