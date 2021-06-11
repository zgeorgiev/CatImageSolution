using Application.APIs;
using Application.Common;
using Application.Services;
using Infrastructure.APIs;
using Infrastructure.Persistent;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IDbContext, ApplicationDbContext>(options => options.UseInMemoryDatabase("CatsDb"));

            services.AddHttpClient("cats", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("CatsUrl").Value);
            });
            services.AddTransient<IImageApi, CatsApi>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
