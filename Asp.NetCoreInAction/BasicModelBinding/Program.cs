using System.Text.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);

builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.AllowTrailingCommas = true;
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.SerializerOptions.PropertyNameCaseInsensitive = true;
});


var app = builder.Build();

app.MapGet("/", () => "Welcome to Basic Model Binding");

app.MapGet("/product/{id}", (ProductId id) => $"Received {id}");

app.MapGet("/products/{id}/paged",
    ([FromRoute] int id,
    [FromQuery] int page,
    [FromHeader(Name = "PageSize")] int pageSize) =>
    $"Received id{id}, page{page}, pageSize {pageSize}");

app.MapPost("/product", (Product product) => $"Received {product}");

app.MapPost("/sizes", (SizeDetails size) => $"Received {size}");

app.MapGet("/category/{id}", (SearchModel model) => $"Received {model}");

app.Run();

public readonly record struct SearchModel(
    [FromRoute] int id,
    [FromQuery] int page,
    [FromHeader(Name = "sort")] bool? sortAsc,
    [FromQuery(Name = "q")] string search);

record Product(string Id, string Name, int Stock);
readonly record struct ProductId(int Id)
{
    public static bool TryParse(string? s, out ProductId result)
    {
        if (s is not null && s.StartsWith("p") && int.TryParse(s.AsSpan().Slice(1), out int id))
        {
            result = new ProductId(id);
            return true;
        }
        result = default;
        return false;
    }
}

public record SizeDetails(double height, double width)
{
    public static async ValueTask<SizeDetails?> BindAsync(HttpContext context)
    {
        using var sr = new StreamReader(context.Request.Body);

        string? line1 = await sr.ReadLineAsync(context.RequestAborted);
        if (line1 is null) { return null; }

        string? line2 = await sr.ReadLineAsync(context.RequestAborted);
        if (line2 is null) { return null; }

        return double.TryParse(line1, out var height)
            && double.TryParse(line2, out var width)
            ? new SizeDetails(height, width)
            : null;
    }
}
