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

        public async Task<Employee> GetEmployeeById()
        {
            return await GetEntity<Employee>().FirstAsync();
        }
    }
}
