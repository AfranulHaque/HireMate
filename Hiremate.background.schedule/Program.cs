
using Hiremate.Background.Scheduler;
using HireMate.DI;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
builder.Services.AddConfigCollections(builder.Configuration);
builder.Services.AddQuartzJobs(builder.Configuration);
builder.Services.AddServiceCollection();

var host = builder.Build();
host.Run();
