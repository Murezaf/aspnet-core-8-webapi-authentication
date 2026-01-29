using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAuthentication.Api;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ApiKeyValidationAuthorizationFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(!IsValidApiKey(context))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    public bool IsValidApiKey(AuthorizationFilterContext context)
    {
        IConfiguration? config = context.HttpContext.RequestServices.GetService<IConfiguration>();
        string? validKey = config.GetValue<string>("Authentication:ApiKey");

        if(string.IsNullOrEmpty(validKey))
        {
            throw new KeyNotFoundException("ApiKey not Found in the configuration.");
        }    

        if(!context.HttpContext.Request.Headers.TryGetValue("X-ApiKey", out var extractedApiKey))
        {
            if(!context.HttpContext.Request.Query.TryGetValue("X-ApiKey", out extractedApiKey))
            {
                return false;
            }
        }

        return validKey.Equals(extractedApiKey);
    }
}
