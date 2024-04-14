using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;
using WebApiWithoutEmpty.Repositories;
using WebApiWithoutEmpty.Services;

namespace WebApiWithoutEmpty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly ShopContext _shopcontext;
        
        public AuthController(IAuthService authService, IConfiguration configuration, IAuthRepository authRepository, ShopContext shopcontext)
        {
            _authservice = authService;
            _configuration = configuration;
            _authRepository = authRepository;
            _shopcontext = shopcontext;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();

            if (await _authservice.UserExists(userForRegisterDto.UserName))
            {
                return BadRequest("user already exists");
            }
            var role = _shopcontext.Roles.FirstOrDefault(x => x.Id == userForRegisterDto.RoleId);
            if (role == null)
            {
                return BadRequest("this role doesnt exit");
            }

            var userrolepair = new UserRole()
            {
                RoleId = role.Id,
            };

            List<UserRole> userrolepairs = new List<UserRole>();
            userrolepairs.Add(userrolepair);

            User user = new()
            {
                UserName = userForRegisterDto.UserName,
                Email = userForRegisterDto.Email,
                userRoles = userrolepairs
            };

            var createduser = await _authservice.Register(user, userForRegisterDto.Password);

            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            var user = await _authRepository.LoginRepo(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,  user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = credentials
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescriptor);

            return Ok(new
            {
                token = tokenhandler.WriteToken(token),
                user
            });
        }
        [Authorize]
        [HttpGet("GetUsersId")]
        public async Task<ActionResult<User>> GetUsersById(int id)
        {
            var users = await _shopcontext.Users.FirstOrDefaultAsync(x=>x.Id == id);
            
            return Ok(users);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await  _shopcontext.Users.ToListAsync();
            return Ok(users);
        }
    }
}
