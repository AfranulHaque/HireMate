using HireMate.DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HireMate.DI
{
    public static class ServiecCollection
    {
        public static IServiceCollection AddExternalCollections(this IServiceCollection services)
        {
          
            return services;
        }
        
        public static IServiceCollection AddPersistenceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<HireMateDBContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
        
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
          
            return services;
        }
    }

}
