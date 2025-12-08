using System.Text.Json.Serialization;
using BizDbAccess.AppStart;
using BizLogic.AppStart;
using BookApp.HelperExtensions;
using BookApp.Logger;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;     // <-- required for IsDevelopment()
using ServiceLayer.AppStart;
using ServiceLayer.BackgroundServices;


var builder = WebApplication.CreateBuilder(args);

// --------------------------
//  Logging
// --------------------------
// builder.Logging.ClearProviders();

// --------------------------
//  Background service
// --------------------------
builder.Services.AddHostedService<BackgroundServiceCountReviews>();
builder.Services.AddControllersWithViews();

// --------------------------
//  MVC + JSON options
// --------------------------
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --------------------------
//  Database
// --------------------------
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EfCoreContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddHttpContextAccessor();

// --------------------------
//  Project DI registrations
// --------------------------
builder.Services.RegisterBizDbAccessDi();
builder.Services.RegisterBizLogicDi();
builder.Services.RegisterServiceLayerDi();

var app = builder.Build();

// --------------------------
//  Logging provider
// --------------------------
var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddProvider(new RequestTransientLogger(() => httpContextAccessor));

// --------------------------
//  Pipeline
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --------------------------
//  Run database setup (your old Startup logic)
// --------------------------
await app.SetupDatabaseAsync();

app.Run();
