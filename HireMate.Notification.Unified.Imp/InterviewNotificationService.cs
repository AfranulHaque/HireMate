using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HireMate.Common;
using HireMate.Common.Utils;
using HireMate.DataManagement.Entities;
using HireMate.DataManagement.Repositories;
using HireMate.Notification.Unified.Interface;

namespace HireMate.Notification.Unified.Imp
{
    public class InterviewNotificationService : IInterviewNotificationService
    {
        public readonly IEventRepository _eventRepository;
        public readonly IEmailService _emailService;

        public InterviewNotificationService(IEventRepository eventRepository, IEmailService emailService)
        {
            _eventRepository = eventRepository;
            _emailService = emailService;
        }
        public async Task SendShortlistAndAvailabilityEmailsAsync()
        {
            string location = "Mohakhali Dohs 177 Road 2";
            var sortedApplicants = await _eventRepository.GetSortedApplicants();
            var employees = await _eventRepository.GetEmployees();
            List<DataManagement.Entities.Employee> sortedEmployee = new();
            employees = employees.Where(_ => sortedEmployee.Any(e => e.EmployeeId == _.EmployeeId)).ToList();
            if (sortedApplicants.Count == 0) return;

            foreach (var applicant in sortedApplicants)
            {
                var r = sortedEmployee.SelectMany(p => p.EmployeeEvents).ToList();
                var t = GetAvailability(sortedEmployee, r);

                string icsContent = EmailHelper.CreateCalendarEntry(DateTime.Now, DateTime.Now, applicant.JobPost.Title, string.Empty, location, new List<string>()
                {
                    applicant.Email
                });

                await _emailService.SendEmailWithIcsAsync(
                    applicant.Email,
                    $"Interview {applicant.JobPost.Title}",
                    $"Dear {applicant.FirstName},<br/><br/>You have been shortlisted for the interview for the position of {applicant.JobPost.Title}.<br/><br/>Please check your calendar for the details.",
                    icsContent
                );
            }
        }




        private string GetAvailability(List<DataManagement.Entities.Employee> interviewers, List<EmployeeEvent> events)
        {
            var bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dhaka");


            for (int dayOffset = 1; dayOffset <= 14; dayOffset++)
            {
                DateTime dateToCheck = DateTime.Today.AddDays(dayOffset);
                dateToCheck = TimeZoneInfo.ConvertTime(dateToCheck, bdTimeZone);

                if (dateToCheck.DayOfWeek == DayOfWeek.Saturday)
                    continue;

                var startOfDay = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 10, 0, 0);
                var endOfDay = new DateTime(dateToCheck.Year, dateToCheck.Month, dateToCheck.Day, 18, 0, 0);

                bool hasAnyFreeSlot = false;

                foreach (var interviewer in interviewers)
                {
                    var eventsForInterviewer = events
                        .Where(e => e.EmployeeId == interviewer.EmployeeId &&
                                    e.EventStartDate.Date == dateToCheck.Date)
                        .OrderBy(e => e.EventStartDate)
                        .ToList();

                    var freeSlots = new List<(DateTime Start, DateTime End)>();
                    DateTime current = startOfDay;

                    foreach (var evt in eventsForInterviewer)
                    {
                        if (evt.EventStartDate > current)
                        {
                            var gap = evt.EventStartDate - current;
                            if (gap.TotalMinutes >= 60)
                                freeSlots.Add((current, evt.EventStartDate));
                        }
                        if (evt.EventEndDate > current)
                            current = evt.EventEndDate;
                    }

                    if (current < endOfDay && (endOfDay - current).TotalMinutes >= 60)
                    {
                        freeSlots.Add((current, endOfDay));
                    }

                    var validSlots = freeSlots
                        .Where(slot => (slot.End - slot.Start).TotalMinutes >= 60)
                        .ToList();

                    if (validSlots.Any())
                    {
                        hasAnyFreeSlot = true;

                        Console.WriteLine($"\n Free 1-hour slots for {interviewer.FirstName} on {dateToCheck:yyyy-MM-dd}:");
                        foreach (var slot in validSlots)
                        {
                            Console.WriteLine($" {slot.Start:hh:mm tt} - {slot.End:hh:mm tt}");
                        }
                    }
                }

                if (hasAnyFreeSlot)
                {
                    return $"Availability found on {dateToCheck:yyyy-MM-dd}";
                }
            }

            return "No 1-hour availability found in the next 14 days (excluding Saturdays).";
        }
    }
}
