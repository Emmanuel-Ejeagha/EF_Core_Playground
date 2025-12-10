using System.Collections.Concurrent;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts =>
    opts.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
    x.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Fruitify",
        Description = "An Api for interacting with fruit stock",
        Version = "1.0",
    }));

var app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/fruit/{id}", (string id) =>
    _fruit.TryGetValue(id, out var fruit)
        ? TypedResults.Ok(fruit)
        : Results.Problem(statusCode: 404))
    .WithTags("fruit")
    .Produces<Fruit>()
    .ProducesProblem(404)
    .WithSummary("Fetches a fruit")
    .WithDescription("Fetches a fruit by id, or returns 404" +
        " if no fruit with ID exists")
    .WithOpenApi(o =>
    {
        o.Parameters[0].Description = "The id of the fruit to fetch";
        o.Summary = "Fetches a fruit";
        return o;
    });

app.MapPost("/fruit/{id}", (string id, Fruit fruit) =>
    _fruit.TryAdd(id, fruit)
        ? TypedResults.Created($"/fruit/{id}", fruit)
        : Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] {"A fruit with this id already exists"}}
        }))
    .WithTags("fruit")
    .Produces<Fruit>(201)
    .ProducesValidationProblem()
    .WithSummary("Adds a fruit")
    .WithDescription("Adds a fruit with id")
    .WithOpenApi(o =>
    {
        o.Parameters[0].Description = "The id of the fruit added";
        o.Summary = "Adds a fruit";
        return o;
    });

app.Run();

record Fruit(string Name, int Stock);