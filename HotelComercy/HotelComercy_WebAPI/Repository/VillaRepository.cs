using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Repository.IRepository;

namespace HotelComercy_WebAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApiVillaContext _context;
        public VillaRepository(ApiVillaContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Villa obj)
        {
            _context.Villas.Update(obj);
        }
    }
}
