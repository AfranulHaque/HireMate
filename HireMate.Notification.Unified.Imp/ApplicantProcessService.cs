using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hiremate.AiAgent.Service.Interface;
using HireMate.Common;
using HireMate.DataManagement.Repositories;
using HireMate.Notification.Unified.Interface;

namespace HireMate.Notification.Unified.Imp
{
    public class ApplicantProcessService : IApplicantProcessService
    {
        public readonly IEventRepository _eventRepository;
        private readonly IAgentService _agentService;
        public ApplicantProcessService(IEventRepository eventRepository, IAgentService agentService)
        {
            _eventRepository = eventRepository;
            _agentService = agentService;
        }
        public async Task ShortListApplicant()
        {
            var applicants = await _eventRepository.GetNewApplicants();
            if (applicants.Count == 0) return;
            foreach (var applicant in applicants)
            {
                var file = Utility.ExtractTextFromPDF(applicant.ResumeFilePath);
                var isEligible = await _agentService.IsCandidateEligable(applicant.JobPost.Description, file);
                applicant.IsShortlisted = isEligible;
                applicant.IsProcessCompleted = !isEligible;
            }
            await _eventRepository.SaveChangesAsync();
        }
        
    }
}
