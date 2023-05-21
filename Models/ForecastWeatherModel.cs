namespace WeatherAPI.Models
{
    /// <summary>
    /// Model for forecase weather data
    /// </summary>
    public class ForecastWeatherModel : IModel
    {
#nullable enable
        public string?[] DayOfWeek { get; set; }
        public string?[] ValidTimeLocal { get; set; }
        public string?[] SunriseTimeLocal { get; set; }
        public string?[] SunsetTimeLocal { get; set; }

        public string?[] Narrative { get; set; }
        public float?[] Qpf { get; set; } // Quantitative Precipitation Forecast: Closest thing to Precip24Hour in current
        public int?[] CalendarDayTemperatureMax { get; set; }
        public int?[] CalendarDayTemperatureMin { get; set; }
        public DayPart?[] DayPart { get; set; }
    }

    public class DayPart // Night and day parts of forecast
    {
        public string?[] DayOrNight { get; set; }
        public int?[] Temperature { get; set; }
        public int?[] UvIndex { get; set; }
        public string?[] WindDirectionCardinal { get; set; }
        public int?[] WindSpeed { get; set; }
        public string?[] VxPhraseLong { get; set; }
    }
}
