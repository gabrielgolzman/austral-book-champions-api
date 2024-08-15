using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public UserDto? GetUser(string email, string password)
        {
            var user = _userRepository.GetUser(email, password);

            return user == null ? null : new UserDto { Email = user.Email};
        }

      
     
    }
}
