    using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data.Configuration;

namespace Api.Extensions
{
    public sealed class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtSettings _settings;

        public ConfigureJwtBearerOptions(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _settings.Issuer,
                ValidateAudience = true,
                ValidAudience = _settings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_settings.SecretKey)
                ),
                ValidateIssuerSigningKey = true
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["access_token"];
                    return Task.CompletedTask;
                }
            };
        }

        public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
    }
}
