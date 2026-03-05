using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;

namespace VaultWebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserRepository _userRepository;

        public AuthService(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _contextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User?> GetAuthenticatedUserAsync()
        {
            HttpRequest? context = _contextAccessor.HttpContext?.Request;
            if (context == null) return null;

            string? token = context.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (token == null) return null;

            User? currentUser = await _userRepository.GetByTokenAsync(token);
            if (currentUser == null) return null;

            return currentUser;
        }
    }
}
