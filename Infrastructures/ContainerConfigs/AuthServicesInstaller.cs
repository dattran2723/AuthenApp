using Commons.Enviroments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructures.ContainerConfigs
{
    public static class AuthServicesInstaller
    {
        public static void ConfigureServicesAuth(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection(nameof(JwtSettings));
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting[nameof(JwtSettings.Secret)] ?? string.Empty));

            // Configure JwtSettings
            services.Configure<JwtSettings>(
                options =>
                {
                    options.Issuer = jwtSetting[nameof(JwtSettings.Issuer)] ?? string.Empty;
                    options.Audience = jwtSetting[nameof(JwtSettings.Audience)] ?? string.Empty;
                    options.ExpirationInHours = int.Parse(jwtSetting[nameof(JwtSettings.ExpirationInHours)] ?? string.Empty);
                    options.RefreshTokenExpirationInDays = int.Parse(jwtSetting[nameof(JwtSettings.RefreshTokenExpirationInDays)] ?? string.Empty);
                    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSetting[nameof(JwtSettings.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtSetting[nameof(JwtSettings.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(
                    configureOptions =>
                    {
                        configureOptions.ClaimsIssuer = jwtSetting[nameof(JwtSettings.Issuer)];
                        configureOptions.TokenValidationParameters = tokenValidationParameters;
                        configureOptions.SaveToken = true;
                    });
        }
    }
}
