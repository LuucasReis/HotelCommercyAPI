using Microsoft.AspNetCore.Identity;

namespace HotelComercy_WebAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
