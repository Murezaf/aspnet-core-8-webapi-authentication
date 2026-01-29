using Microsoft.AspNetCore.Mvc;

namespace WebApiAuthentication.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ApiKeyValidationAuthorizationFilter]
    public IEnumerable<WeatherForecast> Get()
    {
        /*We could simply validate the key when we enter the action before we execute anything else.
        Via the HTTP context variable, we have access to the full request, which means we can get the key from it and validate it,
        but it's quite deep in the request response cycle already. We're in the action. Preferably we don't want to get to that point. 
        We want to block access earlier as everything before this part of the pipeline, like middleware or filters.*/
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
