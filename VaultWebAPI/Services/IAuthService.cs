using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public interface IAuthService
    {
        Task<User> GetAuthenticatedUserAsync();

        string GetRawToken();
    }
}
