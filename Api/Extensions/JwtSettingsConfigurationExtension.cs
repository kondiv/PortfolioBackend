using Data.Configuration;
using Microsoft.Extensions.Options;

namespace Api.Extensions
{
    public static class JwtSettingsConfigurationExtension
    {
        public static IServiceCollection AddJwtSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddSingleton(sp => 
                sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            return services;
        }
    }
}
