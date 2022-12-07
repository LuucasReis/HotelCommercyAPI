using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;

namespace HotelComercy_WebAPI.Repository.IRepository
{
    public interface IUserRepository 
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
