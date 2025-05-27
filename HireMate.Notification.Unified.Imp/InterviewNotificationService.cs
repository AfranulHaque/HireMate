using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.Common;
using HireMate.DataManagement.Repositories;
using HireMate.Notification.Unified.Interface;

namespace HireMate.Notification.Unified.Imp
{
    public class InterviewNotificationService : IInterviewNotificationService
    {
        public readonly IEventRepository _eventRepository;

        public InterviewNotificationService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task SendShortlistAndAvailabilityEmailsAsync()
        {
            var sortedApplicants = await _eventRepository.GetSortedApplicants();
            var employees = await _eventRepository.GetEmployees();
            List<Employee> sortedEmployee = new();
            employees = employees.Where(_ => sortedEmployee.Any(e => e.EmployeeId == _.EmployeeId)).ToList();
            if (sortedApplicants.Count == 0) return;

            foreach (var applicant in sortedApplicants)
            {

            }
        }
    }
}
