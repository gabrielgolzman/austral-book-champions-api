using System;

namespace Infrastructure.Services;

public class TheOneAPIService
{
    private readonly IHttpClientFactory _httpClientFactory = null!;
    public HttpClient theOneAPIClient { get; set; }

    public TheOneAPIService(IHttpClientFactory httpClientFactory)
    {  
        _httpClientFactory = httpClientFactory;
        string? httpClientName = "theOneApiHttpClient";
        theOneAPIClient = _httpClientFactory.CreateClient(httpClientName ?? "");
    }

    public async Task<string?> GetBooks ()
    {
        HttpResponseMessage response = await theOneAPIClient.GetAsync("book");
        string body = await response.Content.ReadAsStringAsync();
        return body;
    }

    public async Task<string?> GetMovies()
    {
        HttpResponseMessage response = await theOneAPIClient.GetAsync("movie");
        string body = await response.Content.ReadAsStringAsync();
        return body;
    }

}
