using Hiremate.AiAgent.Service.Interface;
using HireMate.Common;
using HireMate.DataManagement.Repositories;

namespace HireMate.Service
{
    public class JobService : IJobService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IAgentService _agentService;

        public JobService(IEventRepository eventRepository, IAgentService agentService)
        {
            _eventRepository = eventRepository;
            _agentService = agentService;
        }

        public async Task SaveApplicantDataAsync(Applicant applicant)
        {
            await _eventRepository.AddApplicant(MappingProfile.ToEntity(applicant));

            _eventRepository.SaveChanges();
        }

        public async Task<List<JobPost>> GetAllActiveJobPostAsync()
        {
            var jobPosts = await _eventRepository.GetAllActiveJobPosts();
            return jobPosts.Select(MappingProfile.ToDomain).ToList();
        }

        public async Task GetJobPostByIdAsync(int jobPostId)
        {
            await _eventRepository.GetJobPostById(jobPostId);

            _eventRepository.SaveChanges();
        }

        public async Task AddJobPostAsync(JobPost jobPost)
        {
            await _eventRepository.AddJobPost(MappingProfile.ToEntity(jobPost));

            _eventRepository.SaveChanges();
        }

        public async Task<JobPostAssistantDto> JobPostConversion(JobPostAssistantDto jobPostAssistantDto)
        {
            var response = await _agentService.JobPostAssistant(jobPostAssistantDto);
            return response;
        }
    }
}
