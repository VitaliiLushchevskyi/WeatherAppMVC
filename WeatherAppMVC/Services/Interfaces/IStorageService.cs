namespace WeatherAppMVC.Services.Interfaces;

public interface IStorageService
{
    string GetLastCity();
    void SaveLastCity(string cityName);
    bool HasWarned(string cityName);
    void MarkAsWarned(string cityName);
}
