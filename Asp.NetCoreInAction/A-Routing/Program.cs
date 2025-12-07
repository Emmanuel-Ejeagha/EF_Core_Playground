using A_Routing;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
    options.LoggingFields = HttpLoggingFields.RequestProperties);
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);
builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseUrls = true;
    o.AppendTrailingSlash = true;
    o.LowercaseQueryStrings = false;
});

builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ProductService>();


var app = builder.Build();

app.MapGet("/", (LinkGenerator links, HttpContext context) => GetHomePage(links, context));

app.MapGet("/test", () => "Hello World!").WithName("hello");
app.MapHealthChecks("/healthz").WithName("healthcheck");
// app.MapGet("/HealthCheck", () => Results.Ok()).WithName("healthcheck");
// app.MapGet("/{name}", (string name) => name).WithName("product");
app.MapGet("/{name}", (ProductService service, string name) =>
{
    var product = service.GetProduct(name);
    return product is null
    ? Results.NotFound()
    : Results.Ok(product);
}).WithName("product");

app.MapGet("/redirect-me", () => Results.RedirectToRoute("hello"))
    .WithName("redirect");

// app.MapGet("/", (LinkGenerator links) =>
//     new[]
//     {
//         links.GetPathByName("healthcheck"),
//         links.GetPathByName("product",
//             new { Name = "Big-Widget", Q = "Test"}),
//     });

app.Run();


static string GetHomePage(LinkGenerator links, HttpContext context)
{
    var healthcheck = links.GetPathByName("healthcheck");
    var helloWorld = links.GetPathByName("hello");
    var redirect = links.GetPathByName("redirect");
    var bigWidget = links.GetPathByName("product", new { Name = "big-widget" });
    var fancyWidget = links.GetPathByName(context, "product", new { Name = "super-fancy-widget" });

    return @$"Try navigating to one of the following paths:
        {healthcheck} (standard health check)
        {helloWorld} (Hello world! response)
        {redirect} (Redirects to the {helloWorld} endpoint)
        {bigWidget} or {fancyWidget} (returns the product details)
        /not-a-product (returns a 404)";
}
