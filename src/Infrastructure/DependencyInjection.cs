using Application.APIs;
using Application.Services;
using Infrastructure.APIs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("cats", config =>
            {
                config.BaseAddress = new Uri(configuration.GetSection("CatsUrl").Value);
            });
            services.AddTransient<IImageApi, CatsApi>();
            services.AddTransient<IImageService, ImageService>();
            return services;
        }
    }
}
