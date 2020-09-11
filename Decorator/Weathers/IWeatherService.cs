using System.Threading.Tasks;

namespace DesignPatterns.Decorator.Weathers
{
    interface IWeatherService
    {
        Task<Temperature> GetCurrentTemperature(string location);
        Task<Forecast> GetForcecase(string location);
    }
}
