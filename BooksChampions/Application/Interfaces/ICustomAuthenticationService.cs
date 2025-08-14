using Application.Models.Requests;

namespace Application.Interfaces
{
    public interface ICustomAuthenticationService
    {
        Task<string?> Login(LoginRequest loginRequest);
        Task<bool> Register(RegisterRequest registerRequest);
    }
}
