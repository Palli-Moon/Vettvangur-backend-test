namespace WeatherAPI.Models
{
    public class LocationModel
    {
        public Location? Location { get; set; }
    }

    public class Location
    {
        public string[]? IcaoCode { get; set; }
    }
}
