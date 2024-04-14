using WebApiWithoutEmpty.Entities;

namespace WebApiWithoutEmpty.Repositories
{
    public interface IAuthRepository
    {
        Task<User> LoginRepo(string username, string password);
        Task<User> RegisterRepo(User user, string password);
        Task<bool> UserExistsRepo(string username); 
    }
}
