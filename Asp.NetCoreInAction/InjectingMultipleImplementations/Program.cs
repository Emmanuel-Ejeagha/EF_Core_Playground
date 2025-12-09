using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

// Implement this only in developmentenv bcos it has performance cost
builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateScopes = true;
    o.ValidateOnBuild = true;
});
builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<IMessageSender, FacebookSender>();
builder.Services.AddScoped<IMessageSender, SmsSender>();
builder.Services.TryAddScoped<IMessageSender, UnregisteredSender>();
// builder.Services.AddTransient<DataContext>();
// builder.Services.AddTransient<Repository>();
// builder.Services.AddScoped<DataContext>();
// builder.Services.AddScoped<Repository>();
builder.Services.AddSingleton<DataContext>();
builder.Services.AddSingleton<Repository>();

var app = builder.Build();


// Resolving a scoped Service using IServiceScoe
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    Console.WriteLine($"Retrieved scope: {dbContext.RowCount}");
}

    app.MapGet("/", () => "Try calling /single-message/{username} or /multi-message/{username} and check the logs");
app.MapGet("/single-message/{username}", SendSingleMessage);
app.MapGet("/multi-message/{username}", SendMultiMessage);
app.MapGet("/rowcounts", (DataContext db, Repository repo) =>
    TransientRowCounts(db, repo));
app.MapGet("/scoped", (DataContext db, Repository repo) =>
    ScopedRowCounts(db, repo));
app.MapGet("/singleton", (DataContext db, Repository repo) =>
    SingletonRowCounts(db, repo));


app.Run();

string SendSingleMessage(string username, IMessageSender sender)
{
    sender.SendMessage($"Hello {username}");
    return "Check the application logs to see what was called";
}

string SendMultiMessage(string username, IEnumerable<IMessageSender> senders)
{
    foreach (var sender in senders)
    {
        sender.SendMessage($"Hello {username}");
    }
    return "Check the application logs to see what was called";
}

static string TransientRowCounts(DataContext db, Repository repository)
{
    int dbCount = db.RowCount;
    int repositoryCount = repository.RowCount;

    return $"TransientDataContext: {dbCount}, TransientRepository: {repositoryCount}";
}

static string ScopedRowCounts(DataContext db, Repository repository)
{
    int dbCount = db.RowCount;
    int repositoryCount = repository.RowCount;

    return $"ScopedDataContext: {dbCount}, ScopedRepository: {repositoryCount}";
}

static string SingletonRowCounts(DataContext db, Repository repository)
{
    int dbCount = db.RowCount;
    int repositoryCount = repository.RowCount;

    return $"DataContext: {dbCount}, Repository: {repositoryCount}";
}

interface IMessageSender
{
    void SendMessage(string message);
}

class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending email message; {message}");
    }
}

class FacebookSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending Facebook message: {message}");
    }
}

class SmsSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Sending SMS: {message}");
    }
}

class UnregisteredSender : IMessageSender
{
    public void SendMessage(string message)
    {
        throw new Exception("I'm never registered so shouldn't be called");
    }
}

public class DataContext
{
    public int RowCount { get; }
        = Random.Shared.Next(1, 1_000_000_000);
}

public class Repository
{
    private readonly DataContext _dataContext;
    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public int RowCount => _dataContext.RowCount;
}
