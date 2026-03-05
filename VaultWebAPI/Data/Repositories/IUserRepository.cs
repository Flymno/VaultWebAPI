using VaultWebAPI.Models;

namespace VaultWebAPI.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByTokenAsync(string token);
        Task<string> CreateUser();
        Task<int> RemoveUserAsync(int userId);
    }
}
