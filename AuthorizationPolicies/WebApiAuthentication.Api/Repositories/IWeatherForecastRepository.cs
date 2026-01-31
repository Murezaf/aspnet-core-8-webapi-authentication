namespace WebApiAuthentication.Api.Repositories
{
    public interface IWeatherForecastRepository
    {
        Task<bool> UserCreatedWeatherForecast(string weatherForecastId, string userName);
    }
}