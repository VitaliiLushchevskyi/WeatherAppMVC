using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherAppMVC.Models;
using WeatherAppMVC.Services.Interfaces;

namespace WeatherAppMVC.Controllers;

public class HomeController(IWeatherService weatherService, IStorageService storageService) : Controller
{
    public IActionResult Index()
    {
        string lastCity = storageService.GetLastCity(); 
        ViewBag.LastCity = lastCity;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetWeather(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            ViewBag.Error = "Please enter a city name.";
            return View("Index");
        }

        var weatherInfo = await weatherService.GetWeatherAsync(cityName);
        if (weatherInfo == null)
        {
           ViewBag.Error = "Could not retrieve weather data. Please check the city name.";
           return View("Index");
        }

        storageService.SaveLastCity(cityName);

        bool isRaining = weatherInfo.Conditions.Exists(w => w.Condition.Contains("rain", StringComparison.CurrentCultureIgnoreCase));
        bool alreadyWarned = storageService.HasWarned(cityName); 

        if (isRaining && !alreadyWarned)
        {
            ViewBag.RainWarning = "Warning: It’s going to rain today!";
            storageService.MarkAsWarned(cityName); 
        }

        return View("Index", weatherInfo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
