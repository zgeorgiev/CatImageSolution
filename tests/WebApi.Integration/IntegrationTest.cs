using Application.Common;
using Domain.Extensions;
using Infrastructure.Persistent;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebApi.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(IDbContext));
                    services.AddDbContext<IDbContext, ApplicationDbContext>(options => options.UseInMemoryDatabase("testDB"));
                    
                });
            });

            using (var scope = appFactory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<IDbContext>();
                SeedSampleData(db);
            }
            TestClient = appFactory.CreateClient();
        }

        private void SeedSampleData(IDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new Domain.Entities.User
                {
                    Name = "John Doe",
                    PasswordHash = "demopass".Hash("demouser"),
                    Username = "demouser"
                });

                context.SaveChanges();
            }
        }

        protected void Authenticate()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetBasicAuth());
        }

        private string GetBasicAuth()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes("demouser:demopass"));
        }
    }
}
