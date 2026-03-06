using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/node-tags")]
    [ApiController]
    public class TagsNodesController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TagsNodesController(IAuthService authService)
        {
            _authService = authService;
        }

        //attatch tag to node     POST
        //detatch tag from node   DELETE
        //get tags from node      GET
        //get nodes from tag      GET
    }
}
