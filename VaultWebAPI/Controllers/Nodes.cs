using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;
using VaultWebAPI.DTOs;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class Nodes : ControllerBase
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ITreeService _treeService;
        private readonly IAuthService _authService;

        public Nodes(INodeRepository nodeRepo, ITreeService treeService, IAuthService authService)
        {
            _nodeRepository = nodeRepo;
            _treeService = treeService;
            _authService = authService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromBody] NodeRequest.NodeCreateRequestDTO request)
        {
            User? currentUser = await _authService.GetAuthenticatedUserAsync();
            if (currentUser == null) return Unauthorized();

            Node? newNode = await _nodeRepository.CreateNodeAsync(currentUser.UserId, request.ParentId, request.Name, request.IsCategory);
            if (newNode == null) return StatusCode(500);
            
            return Ok(newNode);
        }

        [HttpGet("getUserTree")]
        public async Task<IActionResult> GetUserNodeTree()
        {
            User? currentUser = await _authService.GetAuthenticatedUserAsync();
            if (currentUser == null) return Unauthorized();

            List<Node>? flatNodes = await _nodeRepository.GetAllUserNodesAsync(currentUser.UserId);
            if (flatNodes == null) return StatusCode(500);

            List<NodeResponse.NodeTreeDTO> userNodeTree = _treeService.BuildNodeTree(flatNodes);

            return Ok(userNodeTree);
        }

        [HttpDelete("{nodeId}")]
        public async Task<IActionResult> DeleteNode(int nodeId)
        {
            User? currentUser = await _authService.GetAuthenticatedUserAsync();
            if (currentUser == null) return Unauthorized();

            int? removedId = await _nodeRepository.DeleteNodeAsync(currentUser.UserId, nodeId);
            if (removedId == null) return NotFound();

            return NoContent();
        }
    }
}
