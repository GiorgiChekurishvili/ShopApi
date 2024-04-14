using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiWithoutEmpty.Dtos;
using WebApiWithoutEmpty.Entities;
using WebApiWithoutEmpty.Repositories;

namespace WebApiWithoutEmpty.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;

        }

        public async Task<UserForLoginDto> Login(string username, string password)
        {
            var user = await _authRepository.LoginRepo(username, password);
            var data = _mapper.Map<UserForLoginDto>(user);
            return data;


        }

        public async Task<UserForRegisterDto> Register(User user, string password)
        {
           var repo = await _authRepository.RegisterRepo(user, password);
            var data = _mapper.Map<UserForRegisterDto>(repo);
            return data;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _authRepository.UserExistsRepo(username);
            return user;
        }

    }
}
