namespace WeatherAPI.Models
{
    /// <summary>
    /// Model for location data. Used in city searching
    /// </summary>
    public class LocationModel : IModel
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public string[] IcaoCode { get; set; }
        public string[] Address { get; set; }
    }
}
