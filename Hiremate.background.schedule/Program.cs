using Hiremate.Background.Scheduler;
using HireMate.DI;
DotNetEnv.Env.Load(".env");
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigCollections(builder.Configuration);
builder.Services.AddServiceCollection();
builder.Services.AddQuartzJobs(builder.Configuration);
builder.Services.AddServiceCollection();
builder.Services.AddPersistenceCollection(builder.Configuration);

var host = builder.Build();
host.Run();
