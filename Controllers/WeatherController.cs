using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.DTO;
using System.Collections.Generic;
using System.Net;
using WeatherAPI.Exceptions;

namespace WeatherAPI.Controllers
{
    /// <summary>
    /// Main weather controller.
    /// </summary>
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

        /// <summary>
        /// Get current weather.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <returns>Weather data</returns>
        [HttpGet]
        [Route("{city}")]
        public async Task<ActionResult<WeatherDTO>> GetCurrentWeather(string city)
        {
            try
            {
                return Ok(await _API.CurrentWeather(city));
            } 
            catch (RequestException ex)
            {
                return StatusCode((int)ex.statusCode, ex.message);
            }
        }

        /// <summary>
        /// Get weather forecast 5 days into the future.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <returns>List of weather data</returns>
        [HttpGet]
        [Route("{city}/forecast")]
        public async Task<ActionResult<WeatherDTO>> GetWeatherForecast(string city)
        {
            try
            {
                return Ok(await _API.WeatherForecast(city));
            } 
            catch (RequestException ex)
            {
                return StatusCode((int)ex.statusCode, ex.message);
            }
        }

        /// <summary>
        /// Get historical weather up to 30 days in the past.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <param name="numberOfDays">Optional. How long into the past to get weather</param>
        /// <returns>List of weather data</returns>
        [HttpGet]
        [Route("{city}/history")]
        [Route("{city}/history/{numberOfDays}")]
        public async Task<ActionResult<HistoryWeatherDTO>> GetHistoricalWeather(string city, int numberOfDays = 30)
        {
            try
            {
                if (numberOfDays < 0 || numberOfDays > 30)
                    return BadRequest("Last parameter has to be a number between 0 and 30. Defaults to 30 if skipped.");

                return Ok(await _API.HistoricalWeather(city, numberOfDays));
            }
            catch (RequestException ex)
            {
                return StatusCode((int)ex.statusCode, ex.message);
            }
        }

        /// <summary>
        /// Displays an error message if the route is not defined
        /// </summary>
        /// <param name="catchAll">Url path of the requested unavailable route</param>
        /// <returns>Bad Request response</returns>
        [HttpGet]
        [Route("/{**catchAll}")]
        public IActionResult CatchAll(string catchAll)
        {
            return BadRequest($"The route '{catchAll}' is not available.");
        }
    }
}
