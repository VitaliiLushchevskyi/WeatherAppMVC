using System.Text.Json;
using WeatherAppMVC.Models.Weather;
using WeatherAppMVC.Services.Interfaces;

namespace WeatherAppMVC.Services.Implementations;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["WeatherApi:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey));
        _baseUrl = configuration["WeatherApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(_baseUrl));
    }

    public async Task<WeatherModel?> GetWeatherAsync(string cityName)
    {
        var url = $"{_baseUrl}?q={cityName}&appid={_apiKey}&units=metric";

        try
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WeatherModel>(json);
        }
        catch
        {
            return null;
        }
    }
}
