using HotelComercy_WebAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelComercy_WebAPI.Data
{
    public class ApiVillaContext : IdentityDbContext<ApplicationUser>
    {
        public ApiVillaContext(DbContextOptions<ApiVillaContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<VillaNumber> VillasNumber { get; set; }
    }
}
