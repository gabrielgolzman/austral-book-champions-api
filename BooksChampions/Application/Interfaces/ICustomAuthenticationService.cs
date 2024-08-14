using Application.Models.Requests;

namespace Application.Interfaces
{
    public interface ICustomAuthenticationService
    {
        string Login(LoginRequest loginRequest);
    }
}
