using HireMate.Common;

namespace HireMate.Service
{
    public interface IJobService
    {
        Task SaveApplicantDataAsync(Applicant applicant);
        Task<List<JobPost>> GetAllActiveJobPostAsync();
        Task GetJobPostByIdAsync(int jobPostId);
        Task AddJobPostAsync(JobPost jobPost);
        Task<JobPostAssistantDto> JobPostConversation(JobPostAssistantDto jobPostAssistantDto);
    }
}
