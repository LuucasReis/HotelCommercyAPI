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
        public Villa Update(Villa obj)
        {
            obj.UpdatedDate = DateTime.Now;
            _context.Villas.Update(obj);
            return obj;
        }
    }
}
