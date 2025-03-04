using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ch10.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase // Defining the WeatherForecastController class inheriting from ControllerBase
    {
        private static readonly string[] Summaries = new[] // Defining a static array of weather summaries
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" // Array elements
        };

        private readonly ILogger<WeatherForecastController> _logger; // Declaring a logger instance

        public WeatherForecastController(ILogger<WeatherForecastController> logger) // Constructor to initialize the logger
        {
            _logger = logger; // Assigning the logger instance
        }

        [HttpGet(Name = "GetWeatherForecast")] // Defining an HTTP GET endpoint
        public IEnumerable<WeatherForecast> Get() // Method to get weather forecasts
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast // Generating a range of 5 weather forecasts
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)), // Setting the date for each forecast
                TemperatureC = Random.Shared.Next(-20, 55), // Generating a random temperature in Celsius
                Summary = Summaries[Random.Shared.Next(Summaries.Length)] // Selecting a random summary
            })
            .ToArray(); // Converting the result to an array
        }
    }
}
