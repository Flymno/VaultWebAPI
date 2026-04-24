using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using VaultWebAPI.Data.Repositories;
using VaultWebAPI.DTOs;
using VaultWebAPI.Models;
using VaultWebAPI.Services;

namespace VaultWebAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepo, IAuthService authService)
        {
            _authService = authService;
            _tagRepository = tagRepo;
        }

        //create tag    POST            DONE
        //delete tag    DELETE
        //update tag    PUT

        [HttpPost("create")]
        public async Task<IActionResult> CreateTag([FromBody] TagCreateRequestDTO request)
        {
            User currentUser = await _authService.GetAuthenticatedUserAsync();

            Tag tag = await _tagRepository.CreateTagAsync(currentUser.UserId, request.Name, request.Color);

            CreatedTagResponseDTO newTag = new CreatedTagResponseDTO(
                tag.TagId,
                tag.Name,
                tag.Color
                );

            return Created($"/api/tags/{tag.TagId}", newTag);
        }
    }
}
