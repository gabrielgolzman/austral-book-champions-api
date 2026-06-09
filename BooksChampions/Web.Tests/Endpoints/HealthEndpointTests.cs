using FluentAssertions;
using System.Net;
using Web.Tests.Helpers;

namespace Web.Tests.Endpoints;

[Trait("Category", "E2E")]
public class HealthEndpointTests : IClassFixture<BooksChampionsApiFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointTests(BooksChampionsApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsOkWithStatus()
    {
        var response = await _client.GetAsync("/api/Book/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("status");
    }
}
