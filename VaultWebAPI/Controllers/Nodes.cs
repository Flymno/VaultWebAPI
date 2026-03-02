using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;
using VaultWebAPI.DTOs;

namespace VaultWebAPI.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class Nodes : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly NodeRepository _nodeRepository;

        public Nodes(UserRepository userRepo, NodeRepository nodeRepo)
        {
            _userRepository = userRepo;
            _nodeRepository = nodeRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromBody] NodeRequest.NodeCreateRequest request)
        {
            User? currentUser = await _userRepository.GetByTokenAsync(request.Token);
            if (currentUser == null) return Unauthorized("Invalid Access Token");

            Node? newNode = await _nodeRepository.CreateNodeAsync(currentUser.UserId, request.ParentId, request.Name);
            if (newNode == null) return StatusCode(500, "Unable to Create Node");
            
            return Ok(newNode);
        }
    }
}
