using System.Net;
using System.Text.Json;
using WeatherAppMVC.Models.Weather;
using WeatherAppMVC.Services.Interfaces;

namespace WeatherAppMVC.Services.Implementations;

public class WeatherService(HttpClient _httpClient, IConfiguration _configuration) : IWeatherService
{
    private readonly string _apiKey = _configuration["WeatherApi:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey));
    private readonly string _baseUrl = _configuration["WeatherApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(_baseUrl));

    public async Task<WeatherModel> GetWeatherAsync(string cityName)
    {
        var url = BuildWeatherApiUrl(cityName);
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new ArgumentException($"City '{cityName}' not found.");
        }
        else if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to retrieve weather data. Status Code: {response.StatusCode}");
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherModel>(json) ?? throw new JsonException("Failed to deserialize weather data.");
    }

    private string BuildWeatherApiUrl(string cityName) => $"{_baseUrl}?q={cityName}&appid={_apiKey}&units=metric";
}