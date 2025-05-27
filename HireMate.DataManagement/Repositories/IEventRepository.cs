using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.DataManagement.Entities;

namespace HireMate.DataManagement.Repositories
{
    public interface IEventRepository : IRepository<EmployeeEvent, long>
    {
        Task<Employee> GetEmployeeById();
    }
}
