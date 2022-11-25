using HotelComercy_WebAPI.Model;

namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        VillaNumber Update(VillaNumber obj);
    }
}
