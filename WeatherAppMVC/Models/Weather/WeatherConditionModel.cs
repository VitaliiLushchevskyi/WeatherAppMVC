using System.Text.Json.Serialization;

namespace WeatherAppMVC.Models.Weather;
public class WeatherConditionModel
{
    [JsonPropertyName("main")]
    public string Condition { get; set; }
}

