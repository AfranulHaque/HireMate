using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return await GetEntity<Applicant>()
                .Where(_ => !_.IsProcessCompleted && _.IsShortlisted)
                .ToListAsync();
        }
    }
}
