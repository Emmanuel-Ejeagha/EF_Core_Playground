using System.Collections.Concurrent;
using System.Net.Mime;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

var _fruit = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/", void () => throw new Exception());

app.MapGet("/fruit", () => _fruit);


app.MapGet("/fruit/{id}", (string id) =>
    _fruit.TryGetValue(id, out var fruit)
    ? TypedResults.Ok(fruit)
    // : Results.NotFound());
    : Results.Problem(statusCode: 404));

app.MapPost("/fruit/{id}", (string id, Fruit fruit) =>
    _fruit.TryAdd(id, fruit)
    ? TypedResults.Created($"/fruit/{id}", fruit)
    // : Results.BadRequest(new
    //     {id = "A fruit with this id already exists"}));
    : Results.ValidationProblem(new Dictionary<string, string[]>
    {
        {"id", new[] {"A fruit with this id already exists"}}        
    }));


app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
{
    _fruit[id] = fruit;
    return Results.NoContent();
});

app.MapDelete("/fruit/{id}", (string id) =>
{
    _fruit.TryRemove(id, out _);
    return Results.NoContent();
});

app.MapGet("/teapot", (HttpResponse response) =>
{
    response.StatusCode = 418;
    response.ContentType = MediaTypeNames.Text.Plain;
    return response.WriteAsync("I' a teapot!");
});

app.Run();



record Fruit(string Name, int Stock);
