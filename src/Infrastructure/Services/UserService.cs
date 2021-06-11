using Application.Common;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContext db;

        public UserService(IDbContext db)
        {
            this.db = db;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var passwordHash = password.Hash(username);
            return await db.GetUserByUsernameAndPasswordAsync(username, passwordHash);
        }

        public async Task<User> GetById(Guid id)
        {
            return await db.GetUserById(id);
        }

        public async Task<User> RegisterAsync(User user)
        {
            return await db.RegisterAsync(user);
        }
    }
}
