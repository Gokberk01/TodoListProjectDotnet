using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Models;
using todoapi.Shared;

namespace todoapi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _UserListcontext;

        public UserRepository(AppDbContext context)
        {
            _UserListcontext = context;
        }

        public async Task CreateAsync(User newUser)
        {   
            await _UserListcontext.UserList.AddAsync(newUser);
            await _UserListcontext.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string userEmail)
        {   
                return await _UserListcontext.UserList.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
        }

        public async Task<User> GetById(int userId)
        {   
                return await _UserListcontext.UserList.FirstOrDefaultAsync(u => u.UserId == userId);
        }


    }
}
