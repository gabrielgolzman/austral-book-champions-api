using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationsServiceOptions _options;

        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticationsServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        private async Task<User?> ValidateUser(LoginRequest rq)
        {
            if (string.IsNullOrEmpty(rq.Email) || string.IsNullOrEmpty(rq.Password))
                return null;

            var user = await _userRepository.GetUser(rq.Email, rq.Password);

            return user;

        }

        public async Task<string?> Login(LoginRequest rq)
        {
            var user = await ValidateUser(rq); 

            if (user == null)
            {
                return null;
            }

            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new("sub", user.Id.ToString()), 
                new ("role", "Cliente")
            };

            var jwtSecurityToken = new JwtSecurityToken( 
              _options.Issuer,
              _options.Audience,
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() 
                .WriteToken(jwtSecurityToken);

            return tokenToReturn.ToString();
        }

        public async Task<bool> Register(RegisterRequest rq)
        {
            if (string.IsNullOrEmpty(rq.Email) || string.IsNullOrEmpty(rq.Password))
                return false;

            var existingUser = await _userRepository.GetUserByEmail(rq.Email);
            if (existingUser != null)
                return false;
                
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(rq.Password);

            var user = new User
            {
                Email = rq.Email,
                Password = hashedPassword
            };

            await _userRepository.CreateUser(user);
            return true;
        }
        public class AuthenticationsServiceOptions
        {
            public const string AuthenticationService = "AuthenticationService";

            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public string? SecretForKey { get; set; }
        }
    }
}
