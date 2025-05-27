using HireMate.DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HireMate.Common;
using Microsoft.Extensions.Options;
using HireMate.Notification.Unified.Imp;
using HireMate.Notification.Unified.Interface;


namespace HireMate.DI
{
    public static class ServiecCollection
    {

        public static IServiceCollection AddConfigCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<AiModel>(configuration.GetSection("AiModel"));
            return services;
        }


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
            services.AddScoped<ICalendarAvailabilityChecker, CalendarAvailabilityChecker>();

            return services;
        }
    }

}
