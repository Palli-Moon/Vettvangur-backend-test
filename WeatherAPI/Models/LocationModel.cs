namespace WeatherAPI.Models
{
    // We are only interested in the IcaoCode here, so we ignore the rest

    public class LocationModel
    {
        public Location? Location { get; set; }
    }

    public class Location
    {
        public string[]? IcaoCode { get; set; }
    }
}
