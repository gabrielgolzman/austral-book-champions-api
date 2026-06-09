using Application.Models.Responses;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace Web.Tests.Helpers;

public class BooksChampionsApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BooksDBConnectionString"] = "Data Source=:memory:",
                ["AuthenticationService:SecretForKey"] = "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)",
                ["AuthenticationService:Issuer"] = "https://localhost:7169",
                ["AuthenticationService:Audience"] = "bookchampionsapi"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BookDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<BookDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }

    public async Task<string> AuthenticateAsync(HttpClient? client = null)
    {
        client ??= CreateClient();
        var email = $"user-{Guid.NewGuid()}@test.com";

        var registerResponse = await client.PostAsJsonAsync("/api/Authentication/register", new
        {
            Username = "testuser",
            Email = email,
            Password = "Password123!"
        });
        registerResponse.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/Authentication/login", new
        {
            Email = email,
            Password = "Password123!"
        });
        loginResponse.EnsureSuccessStatusCode();

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        return auth!.Token!;
    }
}
