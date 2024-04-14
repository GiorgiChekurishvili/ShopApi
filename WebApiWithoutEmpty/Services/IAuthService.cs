using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Services
{
    public interface IAuthService
    {
        Task<UserForRegisterDto> Register(User user, string password);
        Task<UserForLoginDto> Login(string  username, string password);
        Task<bool> UserExists(string username);
    }
}
