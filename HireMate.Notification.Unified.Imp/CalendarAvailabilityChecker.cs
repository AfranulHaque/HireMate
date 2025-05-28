using HireMate.Common;
using HireMate.Notification.Unified.Interface;

namespace HireMate.Notification.Unified.Imp
{
    public class CalendarAvailabilityChecker : ICalendarAvailabilityChecker
    {
        public void GetFreeBusyAsync()
        {
            GetAvailability();
            //GetFromCalendar();
        }

        private string GetAvailability()
        {
            var bdTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dhaka");

            List<Employee> interviewers = new()
                {
                    new Employee { EmployeeId = 1, FirstName = "Alice" },
                    new Employee { EmployeeId = 2, FirstName = "Bob" },
                    //new Employee { EmployeeId = 3, FirstName = "Hitlar" }
                };

            List<Common.Event> events = new()
                {
                    new Common.Event { Id = 1, EmployeeId = 1, EventStartDate = DateTime.Parse("2025-05-28T10:00:00"), EventEndDate = DateTime.Parse("2025-05-28T18:00:00") },
                    new Common.Event { Id = 2, EmployeeId = 1, EventStartDate = DateTime.Parse("2025-05-29T10:00:00"), EventEndDate = DateTime.Parse("2025-05-29T18:00:00") },
                    new Common.Event { Id = 3, EmployeeId = 2, EventStartDate = DateTime.Parse("2025-05-29T15:00:00"), EventEndDate = DateTime.Parse("2025-05-29T16:00:00") },
                    new Common.Event { Id = 4, EmployeeId = 2, EventStartDate = DateTime.Parse("2025-05-28T10:00:00"), EventEndDate = DateTime.Parse("2025-05-28T18:00:00") }
                };

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



        //private string GetFromCalendar()
        //{

        //    var scopes = new[] { CalendarService.Scope.CalendarReadonly };
        //    UserCredential credential;

        //    using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
        //    {
        //        string credPath = "token.json";
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.FromStream(stream).Secrets,
        //            scopes,
        //            "user",
        //            CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //    }

        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "Calendar API .NET Example",
        //    });

        //    var now = DateTime.UtcNow;
        //    var endOfDay = DateTime.UtcNow.Date.AddDays(14);

        //    var request = service.Events.List("primary");
        //    request.TimeMinDateTimeOffset = now;
        //    request.TimeMaxDateTimeOffset = endOfDay;
        //    request.ShowDeleted = false;
        //    request.SingleEvents = true;
        //    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //    var events = request.Execute().Items;

        //    Console.WriteLine("Dummy job executed at: " + DateTime.Now);

        //    return string.Empty;
        //}
    }
}
