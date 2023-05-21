using Newtonsoft.Json.Converters;
using System.Text;
using System.Text.Json;
using WeatherAPI.Models;

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

        public async Task<CurrentWeatherModel> CurrentWeather(string city)
        {
            var location = await GetCityIcaoCode(city);

            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/wx/observations/current",
                Query = $"icaoCode={location}{GetCommonQuery()}"
            };

            return await SendAndDeserialize<CurrentWeatherModel>(builder);
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

            var res = _client.Send(req);
            res.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await res.Content.ReadFromJsonAsync<T>(options) ?? throw new Exception("Error serializing content");
        }

        private async Task<string> GetCityIcaoCode(string city)
        {
            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/location/search",
                Query = $"query={city}&locationType=city{GetCommonQuery(false)}"
            };
            var locations = await SendAndDeserialize<LocationModel>(builder);
            var icaoCodes = locations?.Location?.IcaoCode;

            if (icaoCodes != null && icaoCodes.Length > 0)
                return icaoCodes[0];

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
