using WeatherAppMVC.Services.Interfaces;

namespace WeatherAppMVC.Services.Implementations;

public class StorageService(IHttpContextAccessor httpContextAccessor) : IStorageService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetLastCity()
    {
        return _httpContextAccessor.HttpContext.Request.Cookies["LastCity"] ?? string.Empty;
    }

    public void SaveLastCity(string cityName)
    {
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(1),
            HttpOnly = true,
            Secure = true 
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append("LastCity", cityName, options);
    }

    public bool HasWarned(string cityName)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return false;

        string safeCityName = cityName.Replace(" ", "_"); 
        var warnedTimestamp = context.Request.Cookies[$"Warned_{safeCityName}"];

        if (string.IsNullOrEmpty(warnedTimestamp))
            return false;

        return DateTime.TryParse(warnedTimestamp, out var lastWarnedTime) && lastWarnedTime.AddDays(1) > DateTime.UtcNow;
    }

    public void MarkAsWarned(string cityName)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        string safeCityName = cityName.Replace(" ", "_"); 
        var options = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(1),
            HttpOnly = true,
            Secure = true
        };

        context.Response.Cookies.Append($"Warned_{safeCityName}", DateTime.UtcNow.ToString("o"), options);
    }
}
