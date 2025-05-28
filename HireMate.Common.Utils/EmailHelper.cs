using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net;
using Ical.Net.Serialization;

namespace HireMate.Common.Utils
{
    public static class EmailHelper
    {
        public static string CreateCalendarEntry(DateTime start, DateTime end, string title, string description, string location, List<string> attendees)
        {
            var myEvent = new CalendarEvent

            {
                Summary = title ?? string.Empty,
                Description = description ?? string.Empty,
                Location = location ?? string.Empty,
                Start = new CalDateTime(start),
                End = new CalDateTime(end),
                Organizer = new Organizer()
                {
                    CommonName = "DsiHR",
                    Value = new Uri($"mailto:DsiHR@dsnnovators.com"),
                }
            };
            //Alarm reminder = new Alarm();
            //reminder.Action = AlarmAction.Display;
            //reminder.Trigger = new Trigger(new TimeSpan(-6, 0, 0));
            //myEvent.Alarms.Add(reminder);


            attendees.ForEach(
                _ =>
                {

                    myEvent.Attendees.Add(new Attendee()
                    {
                        CommonName = _,
                        ParticipationStatus = "REQ-PARTICIPANT",
                        Value = new Uri($"mailto:{_}"),
                    });
                }
                );

            Calendar calendar = new Calendar();
            calendar.Events.Add(myEvent);

            var serializer = new CalendarSerializer();
            return serializer.SerializeToString(calendar);
        }

    }
}
