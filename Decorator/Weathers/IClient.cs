
using DesignPatterns.Decorator.Weathers;
using System.Collections.Generic;

namespace DesignPatterns.Decorator
{
    interface IClient
    {
        IEnumerable<string> Events { get; }
        IWeatherService GetWeatherService();
        public int ValueFromCache { get; }
        
    }
}