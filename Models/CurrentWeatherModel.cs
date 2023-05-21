namespace WeatherAPI.Models
{
    /// <summary>
    /// Model for current weather data
    /// </summary>
    public class CurrentWeatherModel : IModel
    {
        public string DayOfWeek { get; set; }
        public string ValidTimeLocal { get; set; }
        public string SunriseTimeLocal { get; set; }
        public string SunsetTimeLocal { get; set; }

        public float Precip24Hour { get; set; }
        public int Temperature { get; set; }
        public int TemperatureMax24Hour { get; set; }
        public int TemperatureMin24Hour { get; set; }
        public int UvIndex { get; set; }
        public string WindDirectionCardinal { get; set; }
        public int WindSpeed { get; set; }
        public string WxPhraseLong { get; set; }
    }
}
