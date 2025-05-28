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
using Hiremate.AiAgent.Service.Interface;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    public class DummyJob : IJob
    {
        private readonly AppSettings _appSettings;
        private readonly IAgentService _agentService;

        public DummyJob(IOptions<AppSettings> appSettings, IAgentService agentService)
        {
            _appSettings = appSettings.Value;
            _agentService = agentService;
        }
        const string jobd = "We are seeking a skilled .NET Developer with 3 years of hands-on experience in developing and maintaining web applications using C#, ASP.NET Core, and Entity Framework. The ideal candidate should have a solid understanding of RESTful APIs, SQL Server, and software development best practices. Experience with front-end technologies like JavaScript or React is a plus. You will collaborate with cross-functional teams to deliver scalable, high-performance solutions. Strong problem-solving skills and a passion for clean, maintainable code are essential.";
        const string jobe = "Skilled Java Developer with 3 years of experience in designing, developing, and maintaining scalable applications using Java, Spring Boot, and Hibernate. Proficient in building RESTful APIs, working with relational databases like MySQL and PostgreSQL, and integrating third-party services. Experienced in applying object-oriented design principles, unit testing with JUnit, and using version control tools like Git. Familiar with microservices architecture and containerization using Docker. Adept at working in agile environments and collaborating closely with cross-functional teams. Strong problem-solving abilities, attention to code quality, and a commitment to continuous learning and delivering high-performance, maintainable solutions. But paralally worked on asp.net with two profetional project";
        public async Task Execute(IJobExecutionContext context)
       {
            await _agentService.IsCandidateEligable(jobd, jobe);
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
