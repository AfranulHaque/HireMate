using Quartz;

namespace Hiremate.Background.Scheduler
{
    public static class QuartzExtensions
    {
        public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var jobConfigurations = configuration.GetSection("QuartzJobConfig:Jobs").Get<List<JobConfiguration>>();

            services.AddQuartz(q =>
            {
                foreach (var jobConfig in jobConfigurations)
                {
                    if (jobConfig.Enabled)
                    {
                        var jobType = Type.GetType(jobConfig.JobType);
                        if (jobType != null)
                        {
                            AddJobAndTrigger(q, jobConfig.JobName, jobType, jobConfig.CronExpression, jobConfig.IntervalInSeconds);
                        }
                    }
                }
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }

        private static void AddJobAndTrigger(IServiceCollectionQuartzConfigurator quartz, string jobName, Type jobType, string cronExpression, int? intervalInSeconds)
        {
            var jobKey = new JobKey(jobName);
            quartz.AddJob(jobType, jobKey, opts => opts.WithIdentity(jobKey));

            if (!string.IsNullOrEmpty(cronExpression))
            {
                quartz.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{jobName}-trigger")
                    .WithCronSchedule(cronExpression));
            }
            else if (intervalInSeconds.HasValue)
            {
                quartz.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity($"{jobName}-trigger")
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalInSeconds.Value).RepeatForever()));
            }
        }

    }
}
