/*

Problem:

    The class "WeatherService" is starting to grow and have to many responsibilitys 

    This will be solved with Decorator pattern

 */

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator.Weathers.Before
{

    class Client : IClient
    {
        private readonly List<string> _events = new List<string>();

        public IEnumerable<string> Events => _events;
        public int ValueFromCache { get; private set; }

        public IWeatherService GetWeatherService()
        {
            var service = new WeatherService();

            service.Log += HandleLog;
            service.GotValueFromCache += HandleGotValueFromCache;

            return service;
        }

        private void HandleGotValueFromCache(object sender, EventArgs e)
        {
            ValueFromCache++;
        }

        private void HandleLog(object sender, string message)
        {
            _events.Add(message);
        }
    }

    class WeatherService : IWeatherService
    {
        readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        readonly List<string> _weatherLogger = new List<string>();

        public EventHandler GotValueFromCache;
        public EventHandler<string> Log;

        public async Task<Temperature> GetCurrentTemperature(string location)
        {
            Log?.Invoke(this, "GetCurrentTemperatur called");

            var cachekey = "GetCurrentTemperatur-" + location;

            var sw = Stopwatch.StartNew();
            var cache = _cache.Get<Temperature>(cachekey);
            if (cache == null)
            {
                await Task.Delay(100);
                var result = location == "Göteborg" ? new Temperature(11) : new Temperature(25);
                _cache.Set(cachekey, result);
            }
            else
            {
                GotValueFromCache?.Invoke(this, null);
            }
            sw.Stop();
            Log?.Invoke(this, "GetCurrentTemperatur ended");
            _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
            return _cache.Get<Temperature>(cachekey);
        }

        public async Task<Forecast> GetForcecase(string location)
        {
            Log?.Invoke(this, "GetForcecase called");

            var cachekey = "GetForcecase-" + location;

            var sw = Stopwatch.StartNew();
            var cache = _cache.Get<Forecast>(cachekey);
            if (cache == null)
            {
                await Task.Delay(100);
                var result = location == "Göteborg" ? new Forecast("Rain") : new Forecast("Sunny");
                _cache.Set(cachekey, result);
            }
            else
            {
                GotValueFromCache?.Invoke(this, null);
            }
            sw.Stop();

            Log?.Invoke(this, "GetForcecase ended");
            _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
            return _cache.Get<Forecast>(cachekey);

        }
    }
}
