using ChangingConvention;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
                .AddRazorPagesOptions(opts =>
                {
                    opts.Conventions.AddPageRouteModelConvention("/Privacy", new PrefixingPageRouteModelConvention().Apply);
                    opts.Conventions.Add(new PageRouteTransformerConvention(new KebabCaseParameterTransformer()));
                    opts.Conventions.AddPageRoute("/ProductDetails/Search", "search-products");
                });

builder.Services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
