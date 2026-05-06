using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheOneAPIController : ControllerBase
    {
        private readonly TheOneAPIService _theOneAPIService;

        public TheOneAPIController(TheOneAPIService theOneAPIService)
        {
            _theOneAPIService = theOneAPIService;
        }

        [HttpGet("books")]
        public async Task<IActionResult> GetBooks()
        {
            string? movies = await _theOneAPIService.GetBooks();
            return Ok(movies);
        }

        [HttpGet("movies")]
        public async Task<IActionResult> GetMovies()    
        {
            string? movies = await _theOneAPIService.GetMovies();
            return Ok(movies);
        }
    }
}
