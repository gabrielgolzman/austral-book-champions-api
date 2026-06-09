using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using static Infrastructure.Services.AuthenticationService;

namespace Infrastructure.Tests.Services;

[Trait("Category", "Unit")]
public class AuthenticationServiceTests
{
    private const string TestSecret = "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)";
    private const string TestIssuer = "https://localhost:7169";
    private const string TestAudience = "bookchampionsapi";

    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly AuthenticationService _sut;

    public AuthenticationServiceTests()
    {
        var options = Options.Create(new AuthenticationsServiceOptions
        {
            SecretForKey = TestSecret,
            Issuer = TestIssuer,
            Audience = TestAudience
        });
        _sut = new AuthenticationService(_userRepositoryMock.Object, options);
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("email@test.com", "")]
    public async Task Login_WithEmptyCredentials_ReturnsNull(string email, string password)
    {
        var result = await _sut.Login(new LoginRequest { Email = email, Password = password });

        result.Should().BeNull();
        _userRepositoryMock.Verify(r => r.GetUser(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Login_WithInvalidUser_ReturnsNull()
    {
        _userRepositoryMock
            .Setup(r => r.GetUser("user@test.com", "wrong"))
            .ReturnsAsync((User?)null);

        var result = await _sut.Login(new LoginRequest { Email = "user@test.com", Password = "wrong" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task Login_WithValidUser_ReturnsJwtWithExpectedClaims()
    {
        var user = new User { Id = 42, Username = "tester", Email = "user@test.com", Password = "hash" };
        _userRepositoryMock
            .Setup(r => r.GetUser("user@test.com", "password"))
            .ReturnsAsync(user);

        var token = await _sut.Login(new LoginRequest { Email = "user@test.com", Password = "password" });

        token.Should().NotBeNullOrEmpty();
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Issuer.Should().Be(TestIssuer);
        jwt.Audiences.Should().Contain(TestAudience);
        jwt.Claims.Should().Contain(c => c.Type == "sub" && c.Value == "42");
        jwt.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Cliente");
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsFalse()
    {
        _userRepositoryMock
            .Setup(r => r.GetUserByEmail("existing@test.com"))
            .ReturnsAsync(new User { Id = 1, Username = "existing", Email = "existing@test.com", Password = "hash" });

        var result = await _sut.Register(new RegisterRequest
        {
            Username = "newuser",
            Email = "existing@test.com",
            Password = "Password123!"
        });

        result.Should().BeFalse();
        _userRepositoryMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsTrue()
    {
        _userRepositoryMock.Setup(r => r.GetUserByEmail("new@test.com")).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(r => r.CreateUser(It.IsAny<User>())).ReturnsAsync((User u) => u);

        var result = await _sut.Register(new RegisterRequest
        {
            Username = "newuser",
            Email = "new@test.com",
            Password = "Password123!"
        });

        result.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.CreateUser(It.Is<User>(u =>
            u.Username == "newuser" &&
            u.Email == "new@test.com" &&
            u.Password.StartsWith("$2"))), Times.Once);
    }
}
