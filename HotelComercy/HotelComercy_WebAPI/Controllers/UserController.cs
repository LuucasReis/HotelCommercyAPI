using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace HotelComercy_WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/UserAuth")]
    [ApiController]
    [ApiVersionNeutral]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApiResponse _apiResponse;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _apiResponse = new();
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            //Está testando se as credenciais do User estão corretas e retornando um JWT Token e o User
            var loginResponse = await _userRepository.Login(loginRequestDTO);
            if(loginResponse.User == null || loginResponse.Token.IsNullOrEmpty())
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages.Add("User ou senha está incorreto");
                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSucess = true;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            //Aqui está testando se o UserName do usuário ja existe no Banco e retornando um booleano
            bool nameIsValid = _userRepository.IsUniqueUser(registrationRequestDTO.UserName);
            if(nameIsValid == false)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages.Add("Este username já existe!!");
                return BadRequest(_apiResponse);
            }

            var User = await _userRepository.Register(registrationRequestDTO);
            if(User == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSucess = false;
                _apiResponse.ErrorMessages.Add("Erro ao registrar!!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSucess = true;
            
            return Ok(_apiResponse);
        }
    }
}
