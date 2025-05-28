using HireMate.DataManagement.Entities;

namespace HireMate.DataManagement.Repositories
{
    public interface IEventRepository : IRepository<EmployeeEvent, long>
    {
        Task<IList<Employee>> GetEmployees();
        Task<EmployeeEvent> GetEmployeeEvent();
        Task<List<Applicant>> GetNewApplicants();
        Task<List<Applicant>> GetSortedApplicants();
        Task<List<Applicant>> GetRejectedApplicants();
        Task AddApplicant(Applicant applicant);
        Task AddEvent(int empId, DateTime date);
    }
}
