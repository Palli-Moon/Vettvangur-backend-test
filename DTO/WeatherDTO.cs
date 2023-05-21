namespace WeatherAPI.DTO
{
    /// <summary>
    /// Common DTO for all routes except historical weather
    /// </summary>
    public class WeatherDTO : BaseWeatherDTO
    {
        public string SunriseTime { get; set; }
        public string SunsetTime { get; set; }
        public int? Temperature { get; set; }
        public int? UvIndex { get; set; }
        public string WindDirection { get; set; }
        public int? WindSpeed { get; set; }
    }
}
