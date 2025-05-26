
using Hiremate.Background.Scheduler;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddQuartzJobs(builder.Configuration);

var host = builder.Build();
host.Run();
