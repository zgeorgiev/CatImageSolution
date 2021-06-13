using Application.APIs;
using Application.Common;
using Application.Services;
using Infrastructure;
using Infrastructure.APIs;
using Infrastructure.Persistent;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    public class DependencySetupFixture
    {
        public DependencySetupFixture()
        {
            var services = new ServiceCollection();
            services.AddDbContext<IDbContext, ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName: "testDB"));
            services.AddHttpClient("cats", config =>
            {
                config.BaseAddress = new Uri("https://cataas.com/");
            });
            services.AddTransient<IImageApi, CatsApi>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IUserService, UserService>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServiceProvider { get; private set; }
    }
}
