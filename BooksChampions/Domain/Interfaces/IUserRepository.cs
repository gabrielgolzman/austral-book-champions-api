using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetUser(string email, string password);
        public Task<User> CreateUser(User user);
        public Task<User?> GetUserByEmail(string email);
    }
}
