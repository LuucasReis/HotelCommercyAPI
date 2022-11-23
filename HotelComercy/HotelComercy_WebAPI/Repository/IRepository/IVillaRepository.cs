using HotelComercy_WebAPI.Model;

namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        void Update(Villa obj);
    }
}
