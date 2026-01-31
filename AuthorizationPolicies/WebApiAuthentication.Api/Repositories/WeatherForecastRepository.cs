namespace WebApiAuthentication.Api.Repositories;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    public async Task<bool> UserCreatedWeatherForecast(string weatherForecastId, string userName)
    {
        return await Task.FromResult(true);
    }
}
