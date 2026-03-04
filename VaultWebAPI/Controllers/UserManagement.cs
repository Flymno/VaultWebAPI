using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.DTOs;

namespace VaultWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserManagement(UserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            string? token = await _userRepository.CreateUser();

            if (token == null) return StatusCode(500);

            return Ok(new { token });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] UserRequest.UserRemoveRequestDTO request)
        {
            if (await _userRepository.RemoveUserAsync(request.Token)) return Ok();
            return StatusCode(500);
        }
    }
}
