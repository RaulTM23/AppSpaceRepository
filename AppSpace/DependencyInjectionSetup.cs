using AppSpace.Business.Interfaces;
using AppSpace.Business.Services;
using AppSpace.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace AppSpace
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<ITheaterManager, TheaterManagerService>();
            services.AddScoped<IViewer, ViewerService>();

            return services;
        }

        public static IServiceCollection CreateDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AppSpace");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection CreateMovieApiHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddHttpClient("movieDbClient", client =>
            {
                client.BaseAddress = new Uri(configuration["MovieDBApi:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(configuration["MovieDBApi:MediaType"]));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(configuration["MovieDBApi:Authentication"], configuration["MovieDBApi:ApiKey"]);

            });

            return services;
        }
    }
}
