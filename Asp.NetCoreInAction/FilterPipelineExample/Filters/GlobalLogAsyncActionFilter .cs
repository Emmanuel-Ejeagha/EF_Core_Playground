using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilterPipelineExample.Filters;

public class GlobalLogAsyncActionFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("Executing GlobalLogAsyncActionFilter - before");

        var executedContext = await next();

        Console.WriteLine($"Executing GloballogAsyncActionFilter - after: cancelled {executedContext.Canceled}");
    }
}
