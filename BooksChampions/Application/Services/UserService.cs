using Application.Models;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetUser(string email, string password)
        {
            var user = await _userRepository.GetUser(email, password);

            return user == null ? null : new UserDto { Email = user.Email};
        }

      
     
    }
}
