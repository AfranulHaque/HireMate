
using HireMate.Common;

namespace HireMate.Service
{
    public interface IJobService
    {
        Task ApplyForJobAsync(Applicant applicant);
        Task<JobPostAssistantDto> JobPostConversion(JobPostAssistantDto jobPostAssistantDto);
    }
}
