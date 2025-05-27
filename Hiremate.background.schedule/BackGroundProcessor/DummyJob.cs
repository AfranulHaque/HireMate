using HireMate.Common;
using Microsoft.Extensions.Options;
using Quartz;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    public class DummyJob : IJob
    {
        private readonly AppSettings _appSettings;

        public DummyJob(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Dummy job executed at: " + DateTime.Now , _appSettings.ApiBaseUrl);
        }
    }
}
