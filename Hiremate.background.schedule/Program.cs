using Hiremate.Background.Scheduler;
using HireMate.DI;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigCollections(builder.Configuration);
builder.Services.AddServiceCollection();
builder.Services.AddQuartzJobs(builder.Configuration);
builder.Services.AddServiceCollection();
builder.Services.AddPersistenceCollection(builder.Configuration);

var host = builder.Build();
host.Run();
