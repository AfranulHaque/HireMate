using HireMate.Common;
using HireMate.DataManagement.Repositories;

namespace HireMate.Service
{
    public class JobService : IJobService
    {
        private readonly IEventRepository _eventRepository;

        public JobService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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
    }
}
