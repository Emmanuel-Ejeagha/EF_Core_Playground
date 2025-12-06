using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

var app = builder.Build();

var _fruit = new ConcurrentDictionary<string, Fruit>();

app.MapGet("/fruit", () => _fruit);

// API without filter
//app.MapGet("/fruit/{id}", (string id) =>
//{
//    if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
//    {
//        return Results.ValidationProblem(new Dictionary<string, string[]>
//        {
//            { nameof(id), new[] { "Invalid format. Id must start with 'f'" } }
//        });
//    }

//    return _fruit.TryGetValue(id, out var fruit)
//            ? TypedResults.Ok(fruit)
//            : Results.Problem(statusCode: 404);
//});

// API using inlined-helper in AddFilter
//app.MapGet("/fruit/{id}", (string id) =>
//    _fruit.TryGetValue(id, out var fruit)
//        ? TypedResults.Ok(fruit)
//        : Results.Problem(statusCode: 404))
//    .AddEndpointFilter(async (context, next) =>
//    {
//        var id = context.GetArgument<string>(0);
//        if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
//        {
//            return Results.ValidationProblem(new Dictionary<string, string[]>
//            {
//                { "id", new[] { "Invalid format. Id must start with 'f'" } }
//            });
//        }
//        return await next(context);
//    });

// API using Helper validation method and additional logging filter
//app.MapGet("/fruit/{id}", (string id) =>
//    _fruit.TryGetValue(id, out var fruit)
//        ? TypedResults.Ok(fruit)
//        : Results.Problem(statusCode: 404))
//    .AddEndpointFilter(ValidationHelper.ValidateId)
//    .AddEndpointFilter(async (context, next) =>
//    {
//        app.Logger.LogInformation("Executing filter...");
//        var result = await next(context);
//        app.Logger.LogInformation($"Handler result: {result}");
//        return result;
//    });


// API using filter factory with helper method
//app.MapGet("/fruit/{id}", (string id) =>
//    _fruit.TryGetValue(id, out var fruit)
//        ? TypedResults.Ok(fruit)
//        : Results.Problem(statusCode: 404))
//    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

// API using IEndpointFilter
app.MapGet("/fruit/{id}", (string id) =>
    _fruit.TryGetValue(id, out var fruit)
        ? TypedResults.Ok(fruit)
        : Results.Problem(statusCode: 404))
    .AddEndpointFilter<IdValidationFilter>();

app.MapPost("/fruit/{id}", (Fruit fruit, string id) =>
    _fruit.TryAdd(id, fruit)
        ? TypedResults.Created($"/fruit/{id}", fruit)
        : Results.ValidationProblem(new Dictionary<string, string[]>
          {
              { "id", new[] { "A fruit with this id already exists" } }
        }))
    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

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

app.Run();

record Fruit(string Name, int stock);

class ValidationHelper
{
    internal static async ValueTask<object?> ValidateId(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<string>(0);
        if (string.IsNullOrEmpty(id) || !id.StartsWith("f"))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "id", new[] { "Invalid format. Id must start with 'f'"}}
            });
        }
        return await next(context);
    }

    internal static EndpointFilterDelegate ValidateIdFactory(
        EndpointFilterFactoryContext context,
        EndpointFilterDelegate next)
    {
        // Find the position of "id" in route handler parameters
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

        // If no "id" parameter, do not apply filter
        if (!idPosition.HasValue)
            return next;

        return async invocationContext =>
        {
            var id = invocationContext.GetArgument<string>(idPosition.Value);

            if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                { "id", new[] { "Invalid format. Id must start with 'f'" } }
                });
            }

            // Continue to next handler
            return await next(invocationContext);
        };
    }

}

    class IdValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<string>(0);
            if (string.IsNullOrEmpty(id) || !id.StartsWith("f"))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "id", new[] {"Invalid format. Id must start wih 'f'"} },
            });
            }
            return await next(context);
        }
    }