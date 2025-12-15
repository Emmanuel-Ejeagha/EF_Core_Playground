using Microsoft.AspNetCore.HttpLogging;
using PageHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts =>
    opts.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<SearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

// app.UseRouting();

// app.UseAuthorization();

// app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.UseStatusCodePagesWithReExecute("/{0}");
app.MapGet("/", () => "Try navigating to /missing to see the page");
app.MapGet("/404", () => "Oops! We couldn't find the page you requested. Please check the url you entered and try again");

app.Run();
