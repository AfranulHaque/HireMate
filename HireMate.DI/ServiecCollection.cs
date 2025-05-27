using System.Runtime;
using AgentApi.Interface;
using Hiremate.AiAgent.Service.Service;
using HireMate.Common;
using HireMate.DataManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HireMate.Notification.Unified.Imp;
using HireMate.Notification.Unified.Interface;
using HireMate.DataManagement.Repositories;
using HireMate.Service;


namespace HireMate.DI
{
    public static class ServiecCollection
    {

        public static IServiceCollection AddConfigCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.Configure<AiModel>(configuration.GetSection("AiModel"));
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
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

            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }

        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<ICalendarAvailabilityChecker, CalendarAvailabilityChecker>();
            services.AddScoped<IInterviewNotificationService, InterviewNotificationService>();
            services.AddScoped<IApplicantProcessService, ApplicantProcessService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRejectedNotificationService, RejectedNotificationService>();

            services.AddScoped<Hiremate.AiAgent.Service.Interface.IAgentService, AgentService>();
            services.AddScoped<IJobService, JobService>();
            return services;
        }
    }

}
