using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonAPIService _pokemonAPIService;
        public PokemonController (PokemonAPIService pokemonAPIService)
        {
            _pokemonAPIService = pokemonAPIService;
        }

        [HttpGet]
        public IActionResult getBerry()
        {
            return Ok(_pokemonAPIService.GetBerry(1));
        }
    }

}
