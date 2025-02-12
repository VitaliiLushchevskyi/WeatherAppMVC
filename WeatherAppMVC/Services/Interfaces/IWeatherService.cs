using WeatherAppMVC.Models.Weather;

namespace WeatherAppMVC.Services.Interfaces;

public interface IWeatherService
{
    Task<WeatherModel> GetWeatherAsync(string cityName);
}

