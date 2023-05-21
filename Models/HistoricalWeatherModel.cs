namespace WeatherAPI.Models
{
    public class HistoricalWeatherModel
    {
#nullable enable
        public string?[] DayOfWeek { get; set; }
        public string?[] ValidTimeLocal { get; set; }
        public float?[] Precip24Hour { get; set; }
        public int?[] TemperatureMax { get; set; }
        public int?[] TemperatureMin { get; set; }
        public string?[] WxPhraseLongDay { get; set; }
        public string?[] WxPhraseLongNight { get; set; }
    }
}
