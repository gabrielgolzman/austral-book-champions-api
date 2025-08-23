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
        public async Task<IActionResult> getBerry()
        {
            return Ok(await _pokemonAPIService.GetBerryAsync(1));
        }

        [HttpPost]
        public async Task<IActionResult> PostBerry()
        {
            return Ok(await _pokemonAPIService.PostBerryAsync("code"));
        }
    }

}
