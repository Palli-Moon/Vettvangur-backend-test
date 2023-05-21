using WeatherAPI.DTO;
using WeatherAPI.Models;
using System.Collections.Generic;

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

        public static IEnumerable<WeatherDTO> Convert(ForecastWeatherModel model, string city = "")
        {
            var dtos = new List<WeatherDTO>();

            for (int i = 0; i < model.DayOfWeek.Length; i++)
            {
                dtos.Add(new WeatherDTO
                {
                    City = city,
                    DayOfWeek = model.DayOfWeek[i],
                    Precipitation = model.Qpf[i],
                    SunriseTime = model.SunriseTimeLocal[i],
                    SunsetTime = model.SunsetTimeLocal[i],
                    Temperature = 0, // TODO
                    TemperatureMax = model.TemperatureMax[i],
                    TemperatureMin = model.TemperatureMin[i],
                    UvIndex = 0, // TODO
                    WindDirection = "", // TODO
                    WindSpeed = 0, // TODO
                    Description = model.Narrative[i] // TODO?
                });
            }

            return dtos;
        }
    }
}
