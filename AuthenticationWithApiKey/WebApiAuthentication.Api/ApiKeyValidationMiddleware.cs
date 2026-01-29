using Microsoft.AspNetCore.Http;
using System.IO.Pipelines;

namespace WebApiAuthentication.Api;

//If access to your API is a matter of all or nothing, in other words, either you have access to the full API or you don't have access at all, then creating a small piece of custom reusable middleware is a pretty good option.
//Moreover, middleware can easily be reused in other projects if you separate it out to a project of its own.
public class ApiKeyValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        IConfiguration config = context.RequestServices.GetRequiredService<IConfiguration>();
        string? validApiKey = config.GetValue<string>("Authentication:ApiKey");

        if (string.IsNullOrWhiteSpace(validApiKey))
        {
            throw new KeyNotFoundException("ApiKey not found in the configuration.");
        }

        if(!context.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
        {
            if(!context.Request.Query.TryGetValue("X-ApiKey", out extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("No ApiKey Provided.");
                return;
            }
        }

        if(!validApiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid ApiKey Provided.");
            return;
        }

        await _next(context);
    }
}
