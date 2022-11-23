using HotelComercy_WebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelComercy_WebAPI.Data
{
    public class ApiVillaContext : DbContext
    {
        public ApiVillaContext(DbContextOptions<ApiVillaContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
    }
}
