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

        #region Helpers
        private async Task<T> SendAndDeserialize<T>(UriBuilder builder)
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
