using HireMate.Common;
using Microsoft.Extensions.Options;
using Quartz;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using HireMate.Notification.Unified.Interface;
using HireMate.Notification.Unified.Imp;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    public class DummyJob : IJob
    {
        private readonly AppSettings _appSettings;
        private readonly ICalendarAvailabilityChecker _calendarAvailabilityChecker;

        public DummyJob(IOptions<AppSettings> appSettings, ICalendarAvailabilityChecker calendarAvailabilityChecker)
        {
            _appSettings = appSettings.Value;
            _calendarAvailabilityChecker = calendarAvailabilityChecker;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            _calendarAvailabilityChecker.GetFreeBusyAsync();

            //var scopes = new[] { CalendarService.Scope.CalendarReadonly };
            //UserCredential credential;

            //using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            //{
            //    string credPath = "token.json";
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.FromStream(stream).Secrets,
            //        scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //}

            //var service = new CalendarService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "Calendar API .NET Example",
            //});

            //var now = DateTime.UtcNow;
            //var endOfDay = DateTime.UtcNow.Date.AddDays(14);

            //var request = service.Events.List("primary");
            //request.TimeMinDateTimeOffset = now;
            //request.TimeMaxDateTimeOffset = endOfDay;
            //request.ShowDeleted = false;
            //request.SingleEvents = true;
            //request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            //var events = request.Execute().Items;
            ////events.First().
            //Console.WriteLine("Dummy job executed at: " + DateTime.Now);
        }

    }
}
