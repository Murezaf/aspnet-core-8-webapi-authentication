using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApiAuthentication.Api.Repositories;

namespace WebApiAuthentication.Api.Authorization;

public class MustHaveCreatedWeatherForecastHandler(IWeatherForecastRepository weatherForecastRepository) 
    : AuthorizationHandler<MustHaveCreatedWeatherForecastRequirement>
{
    private readonly IWeatherForecastRepository _weatherForecastRepository = weatherForecastRepository;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustHaveCreatedWeatherForecastRequirement requirement)
    {
        var httpContext = context.Resource as DefaultHttpContext;
        if (httpContext == null)
        {
            context.Fail();
            return;
        }

        var routeValues = httpContext.GetRouteData().Values;
        if(!routeValues.TryGetValue("id", out var idValue))
        {
            context.Fail();
            return;
        }
        var weatherForecastId = idValue as string;

        var userName = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userName == null || weatherForecastId == null)
        {
            context.Fail();
            return;
        }

        if(!(await _weatherForecastRepository.UserCreatedWeatherForecast(weatherForecastId, userName)))
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}