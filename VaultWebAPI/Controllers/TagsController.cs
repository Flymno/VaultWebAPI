using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TagsController(IAuthService authService)
        {
            _authService = authService;
        }

        //create tag    POST
        //delete tag    DELETE
        //update tag    PUT
    }
}
