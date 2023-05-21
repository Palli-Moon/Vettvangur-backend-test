using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.DTO;
using System.Collections.Generic;

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

        [HttpGet]
        [Route("{city}")]
        public async Task<WeatherDTO> GetCurrentWeather(string city)
        {
            return await _API.CurrentWeather(city);
        }

        [HttpGet]
        [Route("{city}/forecast")]
        public async Task<IEnumerable<WeatherDTO>> GetWeatherForecast(string city)
        {
            return await _API.WeatherForecast(city);
        }

        [HttpGet]
        [Route("{city}/history")]
        public async Task<IEnumerable<BaseWeatherDTO>> GetHistoricalWeather(string city)
        {
            return await _API.HistoricalWeather(city);
        }
    }
}
