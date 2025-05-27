using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.Notification.Unified.Interface;
using Quartz;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    [DisallowConcurrentExecution]
    public class InterviewNotificationJob : IJob
    {
        private readonly IInterviewNotificationService _interviewNotificationService;
        public InterviewNotificationJob(IInterviewNotificationService interviewNotificationService)
        {
            _interviewNotificationService = interviewNotificationService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _interviewNotificationService.SendShortlistAndAvailabilityEmailsAsync();
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Error in ApplicantProccessorJob: {ex.Message}");
            }
        }
    }
}
