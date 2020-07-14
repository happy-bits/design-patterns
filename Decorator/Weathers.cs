/*

DECORATOR PATTERN

Good:

- Logging, Caching and Weather are separated
- It's easy possible to combine them in other orders if you like
(own example)
 */
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace DesignPatterns.Decorator
{
    [TestClass]
    public class Weathers
    {

        [TestMethod]
        public async Task should_be_11_degrees_in_Göteborg()
        {
            var weatherService = new WeatherService();
            var cache = new CachingDecorator(weatherService);
            var ws = new LoggingDecorator(weatherService);

            Temperature temperature = await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(11, temperature.Value);

            CollectionAssert.AreEqual(new[] {
                "GetCurrentTemperatur called",
                "GetCurrentTemperatur ended"
            }, ws.WeatherLoggerWithoutTime.ToArray());
        }


        [TestMethod]
        public async Task should_be_rainy_in_Göteborg()
        {
            var weatherService = new WeatherService();
            var cache = new CachingDecorator(weatherService);
            var ws = new LoggingDecorator(cache);

            Forecast forecast = await ws.GetForcecase("Göteborg");
            Assert.AreEqual("Rain", forecast.Message);

            CollectionAssert.AreEqual(new[] {
                "GetForcecase called",
                "GetForcecase ended"
            }, ws.WeatherLoggerWithoutTime.ToArray());
        }


        [TestMethod]
        public async Task should_get_value_from_cache_two_times()
        {
            var weatherService = new WeatherService();
            var cache = new CachingDecorator(weatherService);
            var ws = new LoggingDecorator(cache);

            Assert.AreEqual(0, cache.GotValueFromCache);

            await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(0, cache.GotValueFromCache);

            await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(1, cache.GotValueFromCache);

            await ws.GetCurrentTemperature("XYZ");
            Assert.AreEqual(1, cache.GotValueFromCache);
        }

        [TestMethod]
        public async Task should_get_value_from_cache_one_time()
        {
            var weatherService = new WeatherService();
            var cache = new CachingDecorator(weatherService);
            var ws = new LoggingDecorator(cache);

            Assert.AreEqual(0, cache.GotValueFromCache);

            await ws.GetForcecase("Göteborg");
            Assert.AreEqual(0, cache.GotValueFromCache);

            await ws.GetForcecase("Göteborg");
            Assert.AreEqual(1, cache.GotValueFromCache);

        }

        class CachingDecorator : IWeatherService
        {
            private readonly IWeatherService _inner;
            readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            public int GotValueFromCache { get; private set; }

            public CachingDecorator(IWeatherService inner)
            {
                _inner = inner;
            }

            public async Task<Temperature> GetCurrentTemperature(string location)
            {
                var cachekey = "GetCurrentTemperatur-" + location;

                var cache = _cache.Get<Temperature>(cachekey);
                if (cache == null)
                {
                    var result = await _inner.GetCurrentTemperature(location);
                    _cache.Set(cachekey, result);
                }
                else
                {
                    GotValueFromCache++;
                }

                return _cache.Get<Temperature>(cachekey);
            }

            public async Task<Forecast> GetForcecase(string location)
            {
                var cachekey = "GetForcecase-" + location;

                var cache = _cache.Get<Forecast>(cachekey);
                if (cache == null)
                {
                    var result = await _inner.GetForcecase(location);
                    _cache.Set(cachekey, result);
                }
                else
                {
                    GotValueFromCache++;
                }
                return _cache.Get<Forecast>(cachekey);
            }
        }
        class LoggingDecorator : IWeatherService
        {
            private readonly IWeatherService _inner;
            private readonly List<string> _weatherLogger = new List<string>();
            public IEnumerable<string> WeatherLoggerWithoutTime => _weatherLogger.Where(w => !w.StartsWith("Time"));

            public LoggingDecorator(IWeatherService inner)
            {
                _inner = inner;
            }

            public Task<Temperature> GetCurrentTemperature(string location)
            {
                _weatherLogger.Add("GetCurrentTemperatur called");
                var sw = Stopwatch.StartNew();

                var result = _inner.GetCurrentTemperature(location);

                sw.Stop();
                _weatherLogger.Add($"GetCurrentTemperatur ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
                return result;
            }

            public Task<Forecast> GetForcecase(string location)
            {
                _weatherLogger.Add("GetForcecase called");
                var sw = Stopwatch.StartNew();

                var result = _inner.GetForcecase(location);

                sw.Stop();
                _weatherLogger.Add($"GetForcecase ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
                return result;
            }
        }

        interface IWeatherService
        {
            Task<Temperature> GetCurrentTemperature(string location);
            Task<Forecast> GetForcecase(string location);
        }

        class WeatherService : IWeatherService
        {
            public async Task<Temperature> GetCurrentTemperature(string location)
            {
                await Task.Delay(100);
                var result = location == "Göteborg" ? new Temperature(11) : new Temperature(25);
                return result;
            }

            public async Task<Forecast> GetForcecase(string location)
            {
                await Task.Delay(100);
                var result = location == "Göteborg" ? new Forecast("Rain") : new Forecast("Sunny");
                return result;
            }
        }

        class Temperature
        {
            public Temperature(int value)
            {
                Value = value;
            }
            public decimal Value { get; }
        }

        class Forecast
        {
            public Forecast(string message)
            {
                Message = message;
            }

            public string Message { get; }
        }
    }
}
