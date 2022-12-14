using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Villa_Utility;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _httpClient;
        private string _villaUrl;
        public VillaNumberService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = _villaUrl + "/api/v1/villaNumberApi",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = _villaUrl + "/api/v1/villaNumberApi/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _villaUrl + "/api/v1/villaNumberApi",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _villaUrl + "/api/v1/villaNumberApi/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = _villaUrl + "/api/v1/villaNumberApi/" + dto.VillaNo,
                Token = token
            });
        }
    }
}
