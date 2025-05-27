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
        public async Task ApplyForJobAsync(Applicant applicant)
        {
            await _eventRepository.AddApplicant(MappingProfile.ToEntity(applicant));
            _eventRepository.SaveChanges();
        }
    }
}
