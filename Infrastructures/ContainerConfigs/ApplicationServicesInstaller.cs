using Abstractions.Services;
using Abstractions.Sessions;
using Commons.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Implementations;
using Services.Mappings;
using System.Reflection;

namespace Infrastructures.ContainerConfigs
{
    public static class ApplicationServicesInstaller
    {
        public static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(DtoEntityAutoMappingProfile).GetTypeInfo().Assembly);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthSession, AuthSession>();
        }
    }
}