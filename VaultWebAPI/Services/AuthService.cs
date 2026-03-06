using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Exceptions;
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

        public async Task<User> GetAuthenticatedUserAsync()
        {
            string token = GetRawToken();

            try { return await _userRepository.GetByTokenAsync(token); }
            catch (NotFoundVaultException) { throw new UnauthorizedVaultException(); }
        }

        public string GetRawToken()
        {
            HttpRequest? context = _contextAccessor.HttpContext?.Request;
            if (context == null) throw new UnauthorizedVaultException();

            string? token = context.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (token == null) throw new UnauthorizedVaultException();

            return token;
        }
    }
}
