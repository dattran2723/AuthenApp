using Entity.Contexts;
using Entity.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures.ContainerConfigs
{
    public static class CoreServicesInstaller
    {
        public static void ConfigureCoreServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(
                options =>
                {
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                });

            services.AddUnitOfWork<AuthDbContext>();
        }
    }
}
