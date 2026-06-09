using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Web.Tests.Helpers;

namespace Web.Tests.Endpoints;

[Trait("Category", "E2E")]
public class BookEndpointTests : IClassFixture<BooksChampionsApiFactory>
{
    private readonly BooksChampionsApiFactory _factory;
    private readonly HttpClient _client;

    public BookEndpointTests(BooksChampionsApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/Book");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostBook_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/Book", new
        {
            Title = "Unauthorized Book",
            Rating = 5,
            PagesAmount = 100,
            IsAvailable = true
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostBook_WithToken_ReturnsCreatedId()
    {
        var token = await _factory.AuthenticateAsync(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/Book", new
        {
            Title = "Authenticated Book",
            Summary = "A test book",
            Rating = 4,
            PagesAmount = 200,
            IsAvailable = true
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var bookId = await response.Content.ReadFromJsonAsync<int>();
        bookId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task DeleteBook_WithToken_RemovesBook()
    {
        var token = await _factory.AuthenticateAsync(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsJsonAsync("/api/Book", new
        {
            Title = "Book To Delete",
            Rating = 3,
            PagesAmount = 120,
            IsAvailable = true
        });
        var bookId = await createResponse.Content.ReadFromJsonAsync<int>();

        var deleteResponse = await _client.DeleteAsync($"/api/Book/{bookId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var listResponse = await _client.GetAsync("/api/Book");
        var books = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        books.GetRawText().Should().NotContain("Book To Delete");
    }
}
