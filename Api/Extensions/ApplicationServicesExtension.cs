using Api.Data;
using Api.Helpers;
using Api.Interfaces;
using Api.Services;
using Api.Services.Interfaces;

namespace Api.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            
            services.Configure<JWT>(configuration.GetSection("JWT"));//Add configuration For JWTSetting Class

            return services;
        }
    }
}
