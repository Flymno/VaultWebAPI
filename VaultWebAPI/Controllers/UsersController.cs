using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;
using VaultWebAPI.Services;
using VaultWebAPI.DTOs;

namespace VaultWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UsersController(IUserRepository userRepo, IAuthService authService)
        {
            _userRepository = userRepo;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            string token = await _userRepository.CreateUser();

            return Created("api/users", new CreatedUserResponseDTO(token));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser()
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            await _userRepository.RemoveUserAsync(currentUser.UserId);

            return NoContent();
        }
    }
}
