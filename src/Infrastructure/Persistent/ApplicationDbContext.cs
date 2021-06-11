using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistent
{
    public class ApplicationDbContext : DbContext, IDbContext
    {

        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<User> RegisterAsync(User user)
        {
            var existingUser = await Users.SingleOrDefaultAsync(x => x.Username == user.Username);
            if (existingUser != null)
            {
                throw new RecordExistException("User already exist in the system");
            }
            Users.Add(user);
            await SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await Users.SingleOrDefaultAsync(x => x.Username == username && x.PasswordHash == password);
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await Users.FindAsync(id);
        }
    }
}
