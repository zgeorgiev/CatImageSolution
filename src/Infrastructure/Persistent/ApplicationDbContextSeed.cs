using Application.Common;
using Domain.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistent
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(IDbContext context)
        {
            // If no users exist in the DB create one default
            if (!context.Users.Any())
            {
                context.Users.Add(new Domain.Entities.User
                {
                    Name = "John Doe",
                    PasswordHash = "demopass".Hash("demouser"),
                    Username = "demouser"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
