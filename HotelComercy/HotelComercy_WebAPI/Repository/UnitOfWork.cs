using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Repository.IRepository;

namespace HotelComercy_WebAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiVillaContext _context;
        public IVillaNumberRepository VillaNumber { get; set; }
        public IVillaRepository Vila { get; set; }

        public UnitOfWork(ApiVillaContext context)
        {
            _context = context;
            Vila = new VillaRepository(_context);
            VillaNumber = new VillaNumberRepository(_context);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
