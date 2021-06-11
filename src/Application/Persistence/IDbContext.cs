using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common
{
    public interface IDbContext
    {
        Task<User> RegisterAsync(User user);
        Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);
        Task<User> GetUserById(Guid id);
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
