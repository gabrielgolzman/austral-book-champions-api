using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repository;
using Infrastructure.Tests.Helpers;

namespace Infrastructure.Tests.Repository;

[Trait("Category", "Integration")]
public class UserRepositoryTests : IDisposable
{
    private readonly Infrastructure.BookDbContext _dbContext;
    private readonly UserRepository _sut;

    public UserRepositoryTests()
    {
        _dbContext = DbContextFactory.Create();
        _sut = new UserRepository(_dbContext);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public async Task GetUser_WithValidCredentials_ReturnsUser()
    {
        var password = "Password123!";
        var user = new User
        {
            Username = "tester",
            Email = "user@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.GetUser("user@test.com", password);

        result.Should().NotBeNull();
        result!.Email.Should().Be("user@test.com");
    }

    [Fact]
    public async Task GetUser_WithWrongPassword_ReturnsNull()
    {
        var user = new User
        {
            Username = "tester",
            Email = "user@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("correct")
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.GetUser("user@test.com", "wrong");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmail_WhenExists_ReturnsUser()
    {
        var user = new User
        {
            Username = "tester",
            Email = "findme@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("pass")
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var result = await _sut.GetUserByEmail("findme@test.com");

        result.Should().NotBeNull();
        result!.Username.Should().Be("tester");
    }
}
