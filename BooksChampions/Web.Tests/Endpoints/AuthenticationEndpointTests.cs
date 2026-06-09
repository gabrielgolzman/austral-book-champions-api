using Application.Models.Responses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Web.Tests.Helpers;

namespace Web.Tests.Endpoints;

[Trait("Category", "E2E")]
public class AuthenticationEndpointTests : IClassFixture<BooksChampionsApiFactory>
{
    private readonly BooksChampionsApiFactory _factory;
    private readonly HttpClient _client;

    public AuthenticationEndpointTests(BooksChampionsApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/Authentication/register", new
        {
            Username = "newuser",
            Email = $"register-{Guid.NewGuid()}@test.com",
            Password = "Password123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        var email = $"duplicate-{Guid.NewGuid()}@test.com";
        var request = new
        {
            Username = "user1",
            Email = email,
            Password = "Password123!"
        };

        await _client.PostAsJsonAsync("/api/Authentication/register", request);
        var response = await _client.PostAsJsonAsync("/api/Authentication/register", new
        {
            Username = "user2",
            Email = email,
            Password = "Password123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/Authentication/login", new
        {
            Email = "nobody@test.com",
            Password = "wrong"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var token = await _factory.AuthenticateAsync(_client);

        token.Should().NotBeNullOrEmpty();
    }
}
