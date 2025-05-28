using HireMate.Notification.Unified.Interface;
using Quartz;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    [DisallowConcurrentExecution]
    public class ApplicantProccessorJob : IJob
    {
        private readonly IApplicantProcessService _applicantProcessService;
        public ApplicantProccessorJob(IApplicantProcessService applicantProcessService)
        {
            _applicantProcessService = applicantProcessService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _applicantProcessService.ShortListApplicant();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ApplicantProccessorJob: {ex.Message}");
            }
        }
    }
}
