using HireMate.DataManagement;
using HireMate.DI;
using Microsoft.EntityFrameworkCore;
using HireMate.Common;
using DotNetEnv;

DotNetEnv.Env.Load(".env");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddExternalCollections();
builder.Services.AddServiceCollection();
builder.Services.AddConfigCollections(builder.Configuration);
builder.Services.AddPersistenceCollection(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
