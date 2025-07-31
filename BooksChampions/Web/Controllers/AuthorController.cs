using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService) {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult Get() {

            return Ok(_authorService.GetAuthors());
        }

    }
}
