using HireMate.DataManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireMate.DataManagement.Repositories
{
    public class EventRepository : Repository<EmployeeEvent, long>, IEventRepository
    {
        public EventRepository(HireMateDBContext context) : base(context)
        {
        }

        public async Task<IList<Employee>> GetEmployees()
        {
            return await GetEntity<Employee>().Include(_ => _.EmployeeEvents).ToListAsync();
        }

        public async Task<EmployeeEvent> GetEmployeeEvent()
        {
            return await GetEntity<EmployeeEvent>()
                .Include(_ => _.Employee).Where(_ => _.EventStartDate >= DateTime.Now)
                .FirstAsync();
        }

        public async Task<List<Applicant>> GetNewApplicants()
        {
            return await GetEntity<Applicant>()
                .Include(_ => _.JobPost)
                .Where(_ => !_.IsProcessCompleted && !_.IsShortlisted)
                .ToListAsync();
        }

        public async Task<List<Applicant>> GetSortedApplicants()
        {
            return await GetEntity<Applicant>().Include(_ => _.JobPost)
                .Where(_ => !_.IsProcessCompleted && _.IsShortlisted)
                .ToListAsync();
        }

        public async Task<List<Applicant>> GetRejectedApplicants()
        {
            return await GetEntity<Applicant>().Include(_ => _.JobPost)
                .Where(_ => !_.IsProcessCompleted && _.IsRejected)
                .ToListAsync();
        }
        
        public async Task AddApplicant(Applicant applicant)
        {
            await GetEntity<Applicant>()
                .AddAsync(applicant);
        }

        public async Task<List<JobPost>> GetAllActiveJobPosts()
        {
            var jobPosts = await GetEntity<JobPost>()
                .Where(_ => _.IsActive)
                .ToListAsync(); 

            return jobPosts ?? new List<JobPost>();
        }

        public async Task<JobPost> GetJobPostById(int jobPostId)
        {
            var jobPost = await GetEntity<JobPost>()
                .FirstOrDefaultAsync(_ => _.JobPostId == jobPostId);

            return jobPost ?? new JobPost();
        }

        public async Task AddJobPost(JobPost jobPost)
        {
            await GetEntity<JobPost>()
                .AddAsync(jobPost);
        }
    }
}
