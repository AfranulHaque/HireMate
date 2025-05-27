using System.Runtime;
using HireMate.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace HireMate.DI
{
    public static class ServiecCollection
    {

        public static IServiceCollection AddConfigCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            return services;
        }


        public static IServiceCollection AddExternalCollections(this IServiceCollection services)
        {
            return services;
        }
        
        public static IServiceCollection AddPersistenceCollection(this IServiceCollection services)
        {
          
            return services;
        }
        
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
          
            return services;
        }
    }
}
