using Application.Common;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class UserServiceTests : IClassFixture<DependencySetupFixture>
    {
        private ServiceProvider _serviceProvide;
        private IUserService _userService;

        public UserServiceTests(DependencySetupFixture fixture)
        {
            _serviceProvide = fixture.ServiceProvider;
            var scope = _serviceProvide.CreateScope();
            _userService = scope.ServiceProvider.GetService<IUserService>();
        }

        [Fact]
        public async Task Register_User_Success()
        {
            var username = Guid.NewGuid().ToString("N");
            var user = new User
            {
                Username = username,
                PasswordHash = Guid.NewGuid().ToString("N").Hash(username),
                Name = "Test user"
            };
            var newUser = await RegisterNewUser(user);
            newUser.Should().NotBeNull();
            newUser.Name.Should().Be("Test user");
        }

        [Fact]
        public async Task Register_User_Already_Exist()
        {
            var username = Guid.NewGuid().ToString("N");
            var user = new User
            {
                Username = username,
                PasswordHash = Guid.NewGuid().ToString("N").Hash(username),
                Name = "Test user"
            };
            var newUser = await RegisterNewUser(user);
            try
            {
                var anotherUser = await RegisterNewUser(newUser);
            }
            catch(RecordExistException ex)
            {
                ex.Should().BeOfType(typeof(RecordExistException));
            }

        }

        [Fact]
        public async Task Authenticate_User_Success()
        {
            // arrange
            var username = Guid.NewGuid().ToString("N");
            var password = Guid.NewGuid().ToString("N");
            var user = new User
            {
                Username = username,
                PasswordHash = password.Hash(username),
                Name = "Test user"
            };
            var newUser = await RegisterNewUser(user);

            // act
            var userDetails = await _userService.AuthenticateAsync(username, password);
            
            // assert
            userDetails.Should().NotBeNull();
        }

        [Fact]
        public async Task Authenticate_User_Wrong_Username_Or_Password()
        {
            // arrange
            var username = Guid.NewGuid().ToString("N");
            var password = Guid.NewGuid().ToString("N");
            var user = new User
            {
                Username = username,
                PasswordHash = password.Hash(username),
                Name = "Test user"
            };
            var newUser = await RegisterNewUser(user);

            // act
            var userDetails = await _userService.AuthenticateAsync(username, Guid.NewGuid().ToString());
            
            // assert
            userDetails.Should().BeNull();
        }

        [Fact]
        public async Task Get_User_By_Id_Success()
        {
            // arrange
            var username = Guid.NewGuid().ToString("N");
            var password = Guid.NewGuid().ToString("N");
            var user = new User
            {
                Username = username,
                PasswordHash = password.Hash(username),
                Name = "Test user"
            };
            var newUser = await RegisterNewUser(user);

            // act
            var dbUser = await _userService.GetById(newUser.Id);

            // assert
            dbUser.Should().NotBeNull();
            dbUser.Id.Should().Be(newUser.Id);
        }

        [Fact]
        public async Task Get_User_By_Id_Not_Exist()
        {
            // arrange
            // act
            var user = await _userService.GetById(Guid.NewGuid());

            // assert
            user.Should().BeNull();
        }

        private async Task<User> RegisterNewUser(User user)
        {
            return await _userService.RegisterAsync(user);
        }
    }
}
