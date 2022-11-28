using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatório!!")]
        [MaxLength(30,ErrorMessage ="O campo {0} deve ter no max {1} caracteres.")]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqtf { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
