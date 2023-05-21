using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly WeatherAPIWrapper _API;

        public WeatherController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _client = clientFactory.CreateClient("Weather.com");
            string apiKey = config.GetValue<string>("ApiKey") ?? throw new Exception("API Key missing. Can not continue");

            _API = new WeatherAPIWrapper(_client, apiKey);
        }

        [HttpGet(Name = "GetWeather")]
        public string Get()
        {

            return "";
        }
    }
}
