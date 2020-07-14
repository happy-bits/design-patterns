/*
 
Same as before but we added logging
 
*/ 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator
{
    [TestClass]
    public class Weathers_NoPattern2
    {
        [TestMethod]
        public async Task should_be_11_degrees_in_Göteborg()
        {
            var ws = new WeatherService();
            var result = await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(11, result.Value);

            CollectionAssert.AreEqual(new[] {
                "GetCurrentTemperatur called",
                "GetCurrentTemperatur ended"
            }, ws.WeatherLoggerWithoutTime.ToArray());
        }

        [TestMethod]
        public async Task should_be_rainy_in_Göteborg()
        {
            var ws = new WeatherService();
            var result = await ws.GetForcecase("Göteborg");
            Assert.AreEqual("Rain", result.Message);

            CollectionAssert.AreEqual(new[] {
                "GetForcecase called",
                "GetForcecase ended"
            }, ws.WeatherLoggerWithoutTime.ToArray());
        }

        class WeatherService
        {
            List<string> _weatherLogger = new List<string>();

            public IEnumerable<string> WeatherLoggerWithoutTime => _weatherLogger.Where(w => !w.StartsWith("Time"));

            public async Task<Temperature> GetCurrentTemperature(string location)
            {
                _weatherLogger.Add("GetCurrentTemperatur called");
                var sw = Stopwatch.StartNew();
                await Task.Delay(100);
                sw.Stop();
                _weatherLogger.Add($"GetCurrentTemperatur ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
                return location == "Göteborg" ? new Temperature(11) : new Temperature(25);
            }

            public async Task<Forecast> GetForcecase(string location)
            {
                _weatherLogger.Add("GetForcecase called");
                var sw = Stopwatch.StartNew();
                await Task.Delay(100);
                _weatherLogger.Add($"GetForcecase ended");
                _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");

                return location == "Göteborg" ? new Forecast("Rain") : new Forecast("Sunny");
            }
        }
    }
}
