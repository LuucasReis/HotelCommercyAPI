using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelComercy_WebAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiVillaContext _context;
        private string _secretKey;

        public UserRepository(ApiVillaContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:PrivateToken");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _context.LocalUsers.FirstOrDefault(x => x.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.LocalUsers.FirstOrDefault(x=> x.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && x.Password == loginRequestDTO.Password);

            if( user == null )
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            /*Basicamente especifica o que o token deve conter, o que no caso, deve conter no token o username com a id,
             Ele deve experirar em 1 dia e ser criptografado conforme especificado no SecurityAlgorithms
             */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            /*Aqui o token será gerado, e necessita das descrições ou regras que especificamos para sua geração
            e isso esta no token descripton que criamos acima*/

            var token = tokenHandler.CreateToken(tokenDescriptor);
            /* Agora para fazer a ligação do token ao prop do DTO, ainda é necessário mais uma linha de código.
              Pois por mais que o token já esteja gerado, ele não esta serializado, e o framework já pensando nisso
            criou uma linha de código, o WriteToken, para serializar o token e transforma-lo em string*/
            LoginResponseDTO response = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };
            return response;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            //Daria pra usar um automapper aqui também, mas como é só em um método...
            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Password = registrationRequestDTO.Password,
                Name = registrationRequestDTO.Name,
                Role = registrationRequestDTO.Role,
            };
            await _context.LocalUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            return user;

        }
    }
}
