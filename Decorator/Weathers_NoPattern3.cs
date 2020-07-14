/*

Same as before but we added caching and added classes for "Temperature" and "Forecast"

Problem:

    The class "WeatherService" is starting to grow and have to many responsibilitys 

    This will be solved with Decorator pattern

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
    public class Weathers_NoPattern3
    {
        [TestMethod]
        public void just_testing_memory_cache()
        {
            MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            _cache.Set("CurrentTemperature-Göteborg", new Temperature(11));
            Temperature d = _cache.Get<Temperature>("CurrentTemperature-Göteborg");
            Temperature d2 = _cache.Get<Temperature>("CurrentTemperature-Oslo");

            Assert.AreEqual(11, d.Value);
            Assert.IsNull(d2);
        }

        [TestMethod]
        public async Task should_be_11_degrees_in_Göteborg()
        {
            var ws = new WeatherService();

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
            var ws = new WeatherService();

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
            var ws = new WeatherService();

            Assert.AreEqual(0, ws.GotValueFromCache);

            await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(0, ws.GotValueFromCache);

            await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(1, ws.GotValueFromCache);

            await ws.GetCurrentTemperature("XYZ");
            Assert.AreEqual(1, ws.GotValueFromCache);
        }



        [TestMethod]
        public async Task should_get_value_from_cache_one_time()
        {
            var ws = new WeatherService();

            Assert.AreEqual(0, ws.GotValueFromCache);

            await ws.GetForcecase("Göteborg");
            Assert.AreEqual(0, ws.GotValueFromCache);

            await ws.GetForcecase("Göteborg");
            Assert.AreEqual(1, ws.GotValueFromCache);

        }

        class WeatherService
        {
            readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            readonly List<string> _weatherLogger = new List<string>();

            public IEnumerable<string> WeatherLoggerWithoutTime => _weatherLogger.Where(w => !w.StartsWith("Time"));

            public int GotValueFromCache { get; private set; }

            public async Task<Temperature> GetCurrentTemperature(string location)
            {
                _weatherLogger.Add("GetCurrentTemperatur called");

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
                    GotValueFromCache++;
                }
                sw.Stop();
                _weatherLogger.Add($"GetCurrentTemperatur ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
                return _cache.Get<Temperature>(cachekey);
            }

            public async Task<Forecast> GetForcecase(string location)
            {
                _weatherLogger.Add("GetForcecase called");

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
                    GotValueFromCache++;
                }
                sw.Stop();

                _weatherLogger.Add($"GetForcecase ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
                return _cache.Get<Forecast>(cachekey);

            }
        }


    }
}
