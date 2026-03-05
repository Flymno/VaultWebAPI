using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public Users(IUserRepository userRepo, IAuthService authService)
        {
            _userRepository = userRepo;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            string? token = await _userRepository.CreateUser();

            if (token == null) return StatusCode(500);

            return Ok(new { token });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUser()
        {
            User? currentUser = await _authService.GetAuthenticatedUserAsync();
            if (currentUser == null) return Unauthorized();

            int? removedId = await _userRepository.RemoveUserAsync(currentUser.UserId);
            if (removedId == null) return NotFound();

            return NoContent();
        }
    }
}
