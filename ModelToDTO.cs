using WeatherAPI.DTO;
using WeatherAPI.Models;

namespace WeatherAPI
{
    public static class ModelToDTO
    {
        public static WeatherDTO Convert(CurrentWeatherModel model, string city = "")
        {
            return new WeatherDTO
            {
                City = city,
                DayOfWeek = model.DayOfWeek,
                Precipitation = model.Precip24Hour,
                SunriseTime = model.SunriseTimeLocal,
                SunsetTime = model.SunsetTimeLocal,
                Temperature = model.Temperature,
                TemperatureMax = model.TemperatureMax24Hour,
                TemperatureMin = model.TemperatureMin24Hour,
                UvIndex = model.UvIndex,
                WindDirection = model.WindDirectionCardinal,
                WindSpeed = model.WindSpeed,
                Description = model.WxPhraseLong
            };
        }

        public static WeatherDTO Convert(ForecastWeatherModel model, string city = "")
        {
            return new WeatherDTO
            {

            };
        }
    }
}
