using System.ComponentModel.DataAnnotations;

namespace HotelComercy_WebAPI.Model.Dto
{
    public class VillaDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatório!!")]
        [MaxLength(30,ErrorMessage ="O campo {0} deve ter no max {1} caracteres.")]
        public string Name { get; set; }
        public string Details { get; set; }
        public int Occupancy { get; set; }
        public int Sqtf { get; set; }
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }

        public VillaDTO()
        {
        }
        public VillaDTO(int id , string name, int occupancy, int sqtf)
        {
            this.id = id;
            Name = name;
            Occupancy = occupancy;
            Sqtf = sqtf;
        }
    }
}
