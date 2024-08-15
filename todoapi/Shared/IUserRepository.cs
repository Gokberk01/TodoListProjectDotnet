using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoapi.Models;

namespace todoapi.Shared
{
    public interface IUserRepository
    {
        Task CreateAsync(User newUser);
        Task<User> GetByEmail(string userEmail);
        Task<User> GetById(int userId);

    }
}