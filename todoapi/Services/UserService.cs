using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todoapi.Models;
using todoapi.Repositories;
using todoapi.Shared;

namespace todoapi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository _repository)
        {
            userRepository = _repository;
        }

        public async Task<User> CreateAsync(User newUser)
        {
            await userRepository.CreateAsync(newUser);
            return newUser;
        }

        public async Task<User> GetByEmail(string userEmail)
        {           
            var user = await userRepository.GetByEmail(userEmail);
            return user;
        }

        public async Task<User> GetById(int userId)
        {           
            var user = await userRepository.GetById(userId);
            return user;
        }

    }
}