using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Tests.Helpers;

namespace Web.Tests.Endpoints;

[Trait("Category", "E2E")]
public class AuthorEndpointTests : IClassFixture<BooksChampionsApiFactory>
{
    private readonly HttpClient _client;

    public AuthorEndpointTests(BooksChampionsApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAuthors_WhenEmpty_ReturnsEmptyArray()
    {
        var response = await _client.GetAsync("/api/Author");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var authors = await response.Content.ReadFromJsonAsync<List<object>>();
        authors.Should().NotBeNull().And.BeEmpty();
    }
}
