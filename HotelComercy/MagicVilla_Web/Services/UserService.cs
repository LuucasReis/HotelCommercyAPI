using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Villa_Utility;

namespace MagicVilla_Web.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaUrl;
        public UserService(IHttpClientFactory httpClientFactory, IConfiguration config) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _villaUrl = config.GetValue<string>("ServiceUrls:VillaApi");
        }
        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = _villaUrl + "/api/v1/UserAuth/login"
            });
        }

        public async Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = _villaUrl + "/api/v1/UserAuth/register"
            });
        }
    }
}
