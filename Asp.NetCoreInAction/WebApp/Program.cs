using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
    options.ListenLocalhost(5001, o => o.UseHttps());
});

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore.httpLogging", LogLevel.Information);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}


app.UseWelcomePage("/welcome");   // <--- Only show welcome at /welcome

app.MapGet("/", () => "Hello World!");
app.MapGet("/person", () => new Person("Emma", "Nuel"));

app.Run();

public record Person(string FirstName, string LastName) { }
