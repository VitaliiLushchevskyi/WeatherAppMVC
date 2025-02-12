using System.Text.Json.Serialization;

namespace WeatherAppMVC.Models.Weather;

public class WeatherModel
{
    [JsonPropertyName("name")]
    public string CityName { get; set; }

    [JsonPropertyName("main")]
    public TemperatureModel Temperature { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherConditionModel> Conditions { get; set; }
}

