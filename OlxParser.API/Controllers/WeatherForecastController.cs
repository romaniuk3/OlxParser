using Microsoft.AspNetCore.Mvc;
using OlxParser.API.Services.Contracts;

namespace OlxParser.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHtmlParseService _htmlParseService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHtmlParseService htmlParseService)
        {
            _logger = logger;
            _htmlParseService = htmlParseService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("parse")]
        public IActionResult Parse()
        {
            string url = "https://www.olx.ua/d/uk/nedvizhimost/kvartiry/dolgosrochnaya-arenda-kvartir/lvov/?currency=UAH&search%5Border%5D=created_at:desc&search%5Bfilter_float_price:from%5D=11999&search%5Bfilter_float_price:to%5D=16001";
            _htmlParseService.Parse(url);

            return Ok();
        }
    }
}