using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using todoapi.Dtos;
using todoapi.Models;
using todoapi.Repositories;
using todoapi.Services;
using todoapi.Shared;

namespace todoTest
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _repository;
        private readonly UserService _service;

        public UserServiceTest()
        {   
            _repository = new Mock<IUserRepository>();

            _service = new UserService(_repository.Object);

        }
        
        [Fact]
        public async Task CreateAsync_ReturnUser()
        {
            //Arrange
            var newRegisterDto = new RegisterDto {UserName = "g", UserEmail = "g@g.com", UserPassword = "g"};
            var newUser = new User 
            {
                UserName = newRegisterDto.UserName,
                UserEmail = newRegisterDto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(newRegisterDto.UserPassword)
            };

            _repository.Setup(s => s.CreateAsync(newUser));

            // Act
            var result = await _service.CreateAsync(newUser);

            // Assert
            Assert.Equal(newUser, result);
        }

        [Fact]
        public async Task GetByEmail_ReturnUser()
        {
            //Arrange
            var newRegisterDtoFirst = new RegisterDto {UserName = "o", UserEmail = "o@o.com", UserPassword = "o"};
            var newRegisterDtoSecond = new RegisterDto {UserName = "g", UserEmail = "g@g.com", UserPassword = "g"};
            List<User> userList = new List<User> 
            {
                new User 
                {                
                    UserName = newRegisterDtoFirst.UserName,
                    UserEmail = newRegisterDtoFirst.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(newRegisterDtoFirst.UserPassword)
                }, 
                new User 
                {
                    UserName = newRegisterDtoSecond.UserName,
                    UserEmail = newRegisterDtoSecond.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(newRegisterDtoSecond.UserPassword)
                }
            };

            _repository.Setup(s => s.GetByEmail("o@o.com")).ReturnsAsync(userList[0]);

            //Act
            var result = await _service.GetByEmail("o@o.com");

            //Assert
            Assert.Equal(userList[0], result);
        }

        [Fact]
        public async Task GetById()
        {
            //Arrange
            var newRegisterDtoFirst = new RegisterDto {UserName = "o", UserEmail = "o@o.com", UserPassword = "o"};
            var newRegisterDtoSecond = new RegisterDto {UserName = "g", UserEmail = "g@g.com", UserPassword = "g"};
            List<User> userList = new List<User> 
            {
                new User 
                {      
                    UserId = 1,          
                    UserName = newRegisterDtoFirst.UserName,
                    UserEmail = newRegisterDtoFirst.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(newRegisterDtoFirst.UserPassword)
                }, 
                new User 
                {
                    UserId = 2,
                    UserName = newRegisterDtoSecond.UserName,
                    UserEmail = newRegisterDtoSecond.UserEmail,
                    UserPassword = BCrypt.Net.BCrypt.HashPassword(newRegisterDtoSecond.UserPassword)
                }
            };

            _repository.Setup(s => s.GetById(1)).ReturnsAsync(userList[0]);
        
            //Act
            var result = await _service.GetById(1);

            //Assert
            Assert.Equal(userList[0], result);
        }
    }
}