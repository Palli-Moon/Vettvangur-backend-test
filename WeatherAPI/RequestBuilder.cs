namespace WeatherAPI
{
    public class RequestBuilder
    {
        private readonly Uri _baseAddress;

        public RequestBuilder(Uri baseAddress)
        {
            _baseAddress = baseAddress;
            var builder = new UriBuilder(_baseAddress)
            {
                Path = "/current?city=london&apiKey=asdf"
            };
        }

        private HttpRequestMessage CreateRequest(UriBuilder builder)
        {
            return new HttpRequestMessage
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Post,
                Content = null
            };
        }
    }
}
