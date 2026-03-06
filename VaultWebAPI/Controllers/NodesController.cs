using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.Models;
using VaultWebAPI.DTOs;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/nodes")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ITreeService _treeService;
        private readonly IAuthService _authService;

        public NodesController(INodeRepository nodeRepo, ITreeService treeService, IAuthService authService)
        {
            _nodeRepository = nodeRepo;
            _treeService = treeService;
            _authService = authService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromBody] NodeCreateRequestDTO request)
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            Node node = await _nodeRepository.CreateNodeAsync(currentUser.UserId, request.ParentId, request.Name, request.IsCategory);

            NodeTreeDTO newNode = new NodeTreeDTO(
                node.NodeId,
                node.ParentId,
                node.IsCategory,
                node.Name,
                null,
                node.DateCreated,
                node.LastModified,
                new List<NodeTreeDTO>()
            );

            return Created($"/api/nodes/{node.NodeId}", newNode);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNodeTree()
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            List<Node> flatNodes = await _nodeRepository.GetAllUserNodesAsync(currentUser.UserId);

            List<NodeTreeDTO> userNodeTree = _treeService.BuildNodeTree(flatNodes);

            return Ok(userNodeTree);
        }

        [HttpDelete("{nodeId}")]
        public async Task<IActionResult> DeleteNode(int nodeId)
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            await _nodeRepository.DeleteNodeAsync(currentUser.UserId, nodeId);

            return NoContent();
        }

        [HttpPut("{nodeId}")]
        public async Task<IActionResult> UpdateNode(int nodeId, [FromBody] NodeUpdateRequestDTO request)
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            Node node = await _nodeRepository.UpdateNodeAsync(currentUser.UserId, nodeId, request.ParentId, request.Name, request.Content);

            NodeTreeDTO newNode = new NodeTreeDTO(
                node.NodeId,
                node.ParentId,
                node.IsCategory,
                node.Name,
                node.Content,
                node.DateCreated,
                node.LastModified,
                new List<NodeTreeDTO>()
            );

            return Ok(newNode);
        }
    }
}
