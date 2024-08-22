using Blog_Api;
using DataLayer.Model_Blog;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Odev_v9.Controllers
{
    public class YazarController : Controller
    {
        private readonly HttpClient _httpClient;

        public YazarController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Makale>> GetMakales()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:5001/api/weatherforecast");
            return JsonConvert.DeserializeObject<IEnumerable<Makale>>(response);
        }

        public async Task<WeatherForecast> PostWeatherForecastAsync(WeatherForecast forecast)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/api/weatherforecast", forecast);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WeatherForecast>();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
