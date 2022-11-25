using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Repository.IRepository;

namespace HotelComercy_WebAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApiVillaContext _context;
        public VillaNumberRepository(ApiVillaContext context) : base(context)
        {
            _context = context;
        }
        public VillaNumber Update(VillaNumber obj)
        {
            obj.UpdatedDate = DateTime.Now;
            _context.VillasNumber.Update(obj);
            return obj;
        }
    }
}
