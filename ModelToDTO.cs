using WeatherAPI.DTO;
using WeatherAPI.Models;
using System.Collections.Generic;

namespace WeatherAPI
{
    public static class ModelToDTO
    {
        /// <summary>
        /// Convert our CurrentWeatherModel to the common WeatherDTO.
        /// Since we are searching for the city it may not yield the expected results, so we display the found city with the DTO.
        /// </summary>
        /// <param name="model">Model for current weather</param>
        /// <param name="city">City that we searched for, since we add it to the DTO</param>
        /// <returns>Weather DTO to be returned to the user</returns>
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

        /// <summary>
        /// Convert ForecastWeatherModel to a list of the common WeatherDTOs.
        /// Regrettably the external API returns an array for every prop that we get, so we loop through them and return them
        /// as a list of the DTO objects instead.
        /// </summary>
        /// <param name="model">Model for weather forecast</param>
        /// <param name="city">City that we searched for, since we add it to the DTO</param>
        /// <returns>List of Weather DTOs to be returned to the user</returns>
        public static IEnumerable<WeatherDTO> Convert(ForecastWeatherModel model, string city = "")
        {
            // Some props are have a day or night value. If it's already night when data is fetched these values are null for today.
            // In this example we display the day version of these values except for today if it's night.

            var isNight = model.DayPart[0]?.DayOrNight[0] == null;
            var dtos = new List<WeatherDTO>();

            for (int i = 0; i < model.DayOfWeek.Length; i++)
            {
                var offset = i == 0 && isNight ? 1 : 0;
                dtos.Add(new WeatherDTO
                {
                    City = city,
                    DayOfWeek = model.DayOfWeek[i],
                    Precipitation = model.Qpf[i],
                    SunriseTime = model.SunriseTimeLocal[i],
                    SunsetTime = model.SunsetTimeLocal[i],
                    Temperature = model.DayPart[0]?.Temperature[i * 2 + offset],
                    TemperatureMax = model.CalendarDayTemperatureMax[i],
                    TemperatureMin = model.CalendarDayTemperatureMin[i],
                    UvIndex = model.DayPart[0]?.UvIndex[i * 2 + offset],
                    WindDirection = model.DayPart[0]?.WindDirectionCardinal[i * 2 + offset],
                    WindSpeed = model.DayPart[0]?.WindSpeed[i * 2 + offset],
                    Description = model.Narrative[i]
                });
            }

            return dtos;
        }
    }
}
