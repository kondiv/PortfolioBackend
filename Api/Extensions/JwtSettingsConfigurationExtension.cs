using Data.Configuration;

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

            return services;
        }
    }
}
