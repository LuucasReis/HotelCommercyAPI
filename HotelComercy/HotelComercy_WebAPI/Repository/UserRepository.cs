using AutoMapper;
using HotelComercy_WebAPI.Data;
using HotelComercy_WebAPI.Model;
using HotelComercy_WebAPI.Model.Dto;
using HotelComercy_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelComercy_WebAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiVillaContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string _secretKey;

        public UserRepository(ApiVillaContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager
            , IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:PrivateToken");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if(user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if(user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            /*Basicamente especifica o que o token deve conter, o que no caso, deve conter no token o username com a id,
             Ele deve experirar em 1 dia e ser criptografado conforme especificado no SecurityAlgorithms
             */
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    //usando first or default porque só tem uma ROle, se tivesse mais de uma, faria um foreach para adicionar todas
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
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
                User = _mapper.Map<UserDTO>(user),
            };
            return response;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            //Daria pra usar um automapper aqui também, mas como é só em um método...
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper(),
                Name = registrationRequestDTO.Name,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        //Geralmente eu Faria isso no Seeding Service, mas como implementei o Identity depois e é para fins de estudo, deixei assim.
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                
                    var userReturn = _context.ApplicationUsers.FirstAsync(x => x.UserName == registrationRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userReturn);
                }
            }
            catch (Exception e)
            {

            }
            return new UserDTO();

        }
    }
}
