using System.Collections.Concurrent;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

var app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

RouteGroupBuilder fruitApi = app.MapGroup("/fruit");

fruitApi.MapGet("/", () => _fruit);

RouteGroupBuilder fruitApiWithValidation = fruitApi.MapGroup("/")
    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

fruitApiWithValidation.MapGet("/{id}", (string id) =>
    _fruit.TryGetValue(id, out var fruit)
        ? TypedResults.Ok(fruit)
        : Results.Problem(statusCode: 404));

fruitApiWithValidation.MapPost("/{id}", (Fruit fruit, string id) =>
    _fruit.TryAdd(id, fruit)
        ? TypedResults.Created($"/fruit/{id}", fruit)
        : Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { "id", new[] { "A fruit with this id already exists"} }
        }));

fruitApiWithValidation.MapPut("/{id}", (string id, Fruit fruit) =>
{
    _fruit[id] = fruit;
    return Results.NoContent();
});

fruitApiWithValidation.MapDelete("/{id}", (string id) =>
{
    _fruit.TryRemove(id, out _);
    return Results.NoContent();
});


app.Run();

record Fruit(string Name, int Stock) { }

class ValidationHelper
{
    internal static EndpointFilterDelegate ValidateIdFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        var parameters = context.MethodInfo.GetParameters();
        int? idPosition = null;

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].Name == "id" && parameters[i].ParameterType == typeof(string))
            {
                idPosition = i;
                break;
            }
        }

        if (!idPosition.HasValue)
            return next;

        return async invocationContext =>
        {
            string id = invocationContext.GetArgument<string>(idPosition.Value);

            if (string.IsNullOrEmpty(id) || !id.StartsWith("f"))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    { "id", new[] {"Invalid format. Id must start with 'f'"} },
                });
            }

            return await next(invocationContext);
        };
    }
}