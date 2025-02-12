using System.Text.Json.Serialization;

namespace WeatherAppMVC.Models.Weather;

public class TemperatureModel
{
    [JsonPropertyName("temp_max")]
    public double MaxTemperature { get; set; }

    [JsonPropertyName("temp_min")]
    public double MinTemperature { get; set; }
}

