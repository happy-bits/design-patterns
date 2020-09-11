/*

Same as before but we added caching and added classes for "Temperature" and "Forecast"

Problem:

    The class "WeatherService" is starting to grow and have to many responsibilitys 

    This will be solved with Decorator pattern

 */

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator.Weathers
{
    interface IWeatherService
    {
        //int GotValueFromCache { get; }
        //IEnumerable<string> WeatherLoggerWithoutTime { get; }

        Task<Temperature> GetCurrentTemperature(string location);
        Task<Forecast> GetForcecase(string location);
    }
}

//    Task<Temperature> GetCurrentTemperature(string location);
//    Task<Forecast> GetForcecase(string location);