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
            var location = await GetCity(city);

            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/v3/wx/observations/current",
                Query = $"icaoCode={location.Item1}{GetCommonQuery()}"
            };

            var res = await SendAndDeserialize<CurrentWeatherModel>(builder);
            res.City = location.Item2;

            return res;
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
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await res.Content.ReadFromJsonAsync<T>(options) ?? throw new Exception("Error serializing content");
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
