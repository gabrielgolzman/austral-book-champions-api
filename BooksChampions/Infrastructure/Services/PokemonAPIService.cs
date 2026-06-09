using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PokemonAPIService
    {
        public async Task<string>GetBerry(int id)
        {
            HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync($"https://pokeapi.co/api/v2/berry/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
