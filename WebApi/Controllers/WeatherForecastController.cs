using Microsoft.AspNetCore.Mvc;
using TestNet8.Model;
using TestNet8.WebApi.Metrix;

namespace TestNet8.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherMetrics _metrics;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            WeatherMetrics metrics)
        {
            _logger = logger;
            _metrics = metrics;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("hello weather");

            using var _ = _metrics.MeasureRequestDuration();
            try {
                await Task.Delay(Random.Shared.Next(5, 100));
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
            finally {
                _metrics.IncreaseRequestCount();
            }
        }
    }
}
