using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetById(Guid id);
    }
}
