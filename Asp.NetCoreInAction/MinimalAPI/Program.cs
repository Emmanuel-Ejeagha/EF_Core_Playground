using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}

// Note that this is for demonstration only
// Using a List is not thread-safe and should not be used in 
// practice. Instead, use a database or a thread-safe data structure
var _people = new List<Person>
{
    new("Tom", "Dike"),
    new("Chally", "onu"),
    new("Jerry", "Ude"),
    new("Uriel", "David"),
    new("Loveth", "Ogbodo")
};

app.MapGet("/", () => "Hello World!");
app.MapGet("/error", () => "Sorry an error occurred");
app.MapGet("/person/{name}", (string name) =>
    _people.Where(p => p.FirstName.StartsWith(name, StringComparison.OrdinalIgnoreCase)));

app.Run();

record Person(string FirstName, string LastName);
