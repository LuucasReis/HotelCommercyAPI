using System.ComponentModel.DataAnnotations;

namespace HotelComercy_WebAPI.Model.Dto
{
    public class VillaDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatório!!")]
        [MaxLength(30,ErrorMessage ="O campo {0} deve ter no max {1} caracteres.")]
        public string Name { get; set; }

        public VillaDTO()
        {
        }
        public VillaDTO(int id , string name)
        {
            this.id = id;
            Name = name;
        }
    }
}
