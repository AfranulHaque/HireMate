using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.DataManagement.Repositories;
using HireMate.Notification.Unified.Interface;

namespace HireMate.Notification.Unified.Imp
{
    public class RejectedNotificationService : IRejectedNotificationService
    {
        public readonly IEventRepository _eventRepository;
        public readonly IEmailService _emailService;

        public RejectedNotificationService(IEventRepository eventRepository, IEmailService emailService)
        {
            _eventRepository = eventRepository;
            _emailService = emailService;
        }

        public async Task SendRejectedEmail()
        {
            var applicants = await _eventRepository.GetRejectedApplicants();
            if (applicants.Count == 0) return;
            foreach (var applicant in applicants)
            {
                await _emailService.SendEmailWithIcsAsync(
                  applicant.Email,
                  $"Interview {applicant.JobPost.Title}",
                  $"Dear {applicant.FirstName},<br/><br/>We appreciate your interest in the position of {applicant.JobPost.Title}.<br/><br/>After careful consideration, we regret to inform you that you have not been shortlisted for this role.<br/><br/>We encourage you to apply for future opportunities that match your profile.<br/><br/>Thank you once again for your application."
                  , null
              );
                applicant.IsProcessCompleted = true;
            }
            await _eventRepository.SaveChangesAsync();
        }
    }
}
