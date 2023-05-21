using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.DTO;
using WeatherAPI.Models;
using System.Collections.Generic;

namespace WeatherAPI
{
    /// <summary>
    /// Wrapper for the weather.com external API
    /// </summary>
    public class WeatherAPIWrapper
    {
        private readonly Uri _baseAddress;
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public WeatherAPIWrapper(HttpClient client, string apiKey)
        {
            _client = client;
            _baseAddress = client.BaseAddress ?? throw new Exception("Could not find base address for external API");
            _apiKey = apiKey;
        }

        /// <summary>
        /// Used by the /weather/{city} route. Gets current weather from the external API.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <returns>Weather data</returns>
        public async Task<WeatherDTO> CurrentWeather(string city)
        {
            var location = await GetCity(city);

            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/wx/observations/current",
                Query = $"icaoCode={location.Item1}{GetCommonQuery()}"
            };

            var res = await SendAndDeserialize<CurrentWeatherModel>(builder);
            return ModelToDTO.Convert(res, location.Item2);
        }

        /// <summary>
        /// Used by the /weather/{city}/forecast route. Gets forecast for weather 5 days in the future.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <returns>List of weather data</returns>
        public async Task<IEnumerable<WeatherDTO>> WeatherForecast(string city)
        {
            var location = await GetCity(city);

            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/wx/forecast/daily/5day",
                Query = $"icaoCode={location.Item1}{GetCommonQuery()}"
            };

            var res = await SendAndDeserialize<ForecastWeatherModel>(builder);
            return ModelToDTO.Convert(res, location.Item2);
        }

        /// <summary>
        /// Used by the /weather/{city}/history/{numberOfDays} route. Gets historical weather up to 30 days in the past.
        /// </summary>
        /// <param name="city">City to get weather for</param>
        /// <param name="numberOfDays">How far into the past we display data</param>
        /// <returns>List of weather data</returns>
        public async Task<IEnumerable<HistoryWeatherDTO>> HistoricalWeather(string city, int numberOfDays)
        {
            var location = await GetCity(city);

            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/wx/conditions/historical/dailysummary/30day",
                Query = $"icaoCode={location.Item1}{GetCommonQuery()}"
            };

            var res = await SendAndDeserialize<HistoricalWeatherModel>(builder);
            return ModelToDTO.Convert(res, location.Item2, numberOfDays);
        }

        #region Helpers

        /// <summary>
        /// Sends a prepare request to the external API. It then deserializes the JSON data into the correct model format.
        /// </summary>
        /// <typeparam name="T">Type of model to serialize to</typeparam>
        /// <param name="builder">Uri builder with data to send request</param>
        /// <returns>Serialized model</returns>
        private async Task<T> SendAndDeserialize<T>(UriBuilder builder) where T : IModel
        {
            var req = new HttpRequestMessage
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Get,
                Content = null
            };

            var res = await _client.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var resString = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<T>(resString, options);
        }

        /// <summary>
        /// Queries API with city param to get location data to use in other requests. The result is included in DTOs
        /// to make sure that the resulting city is correct. If more than one city is found, only the top result is used.
        /// </summary>
        /// <param name="city">City to be searched for</param>
        /// <returns>A tuple containing the location data and top search result</returns>
        /// <exception cref="Exception">Thrown if city was not found</exception>
        private async Task<Tuple<string, string>> GetCity(string city)
        {
            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/location/search",
                Query = $"query={city}&locationType=city{GetCommonQuery(false)}"
            };
            var locations = await SendAndDeserialize<LocationModel>(builder);
            var icaoCodes = locations?.Location?.IcaoCode;
            var address = locations?.Location?.Address;

            if (icaoCodes != null && address != null && icaoCodes.Length > 0)
                return new Tuple<string, string>(icaoCodes[0], address[0]);

            throw new Exception($"The location {city} was not found");
        }

        /// <summary>
        /// Helper to create the query string since all requests have some things in common.
        /// </summary>
        /// <param name="includeUnits">Whether to include the units prop or not since it's not always used</param>
        /// <returns>A string to add to the query</returns>
        private string GetCommonQuery(bool includeUnits = true)
        {
            var query = $"&language=en-US&format=json&apiKey={_apiKey}";
            if (includeUnits)
                query += "&units=m";
            return query;
        }
        #endregion
    }
}
