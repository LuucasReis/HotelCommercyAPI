﻿using System.ComponentModel.DataAnnotations;

namespace HotelComercy_WebAPI.Model.Dto
{
    public class VillaNumberCreateDTO
    {

        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
