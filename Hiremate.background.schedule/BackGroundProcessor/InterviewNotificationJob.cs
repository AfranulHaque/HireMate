using HireMate.Notification.Unified.Interface;
using Quartz;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    [DisallowConcurrentExecution]
    public class InterviewNotificationJob : IJob
    {
        private readonly IInterviewNotificationService _interviewNotificationService;
        private readonly IRejectedNotificationService _rejectedNotificationService;
        public InterviewNotificationJob(IInterviewNotificationService interviewNotificationService, IRejectedNotificationService rejectedNotificationService)
        {
            _interviewNotificationService = interviewNotificationService;
            _rejectedNotificationService = rejectedNotificationService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _interviewNotificationService.SendShortlistAndAvailabilityEmailsAsync();
                await _rejectedNotificationService.SendRejectedEmail();
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Error in ApplicantProccessorJob: {ex.Message}");
            }
        }
    }
}
