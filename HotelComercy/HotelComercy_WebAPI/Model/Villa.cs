using System.ComponentModel.DataAnnotations;

namespace HotelComercy_WebAPI.Model
{
    public class Villa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório!!")]
        [MaxLength(30, ErrorMessage = "O campo {0} deve ter no max {1} caracteres.")]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
