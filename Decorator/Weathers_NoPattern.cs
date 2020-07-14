// Own example (stole weather-idea from David Berry)
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator
{
    [TestClass]
    public class Weathers_NoPattern
    {
        [TestMethod]
        public async Task should_be_11_degrees_in_Göteborg()
        {
            var ws = new WeatherService();
            decimal d = await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(11, d);
        }

        [TestMethod]
        public async Task should_be_rainy_in_Göteborg()
        {
            var ws = new WeatherService();
            string s = await ws.GetForcecase("Göteborg");
            Assert.AreEqual("Rain", s);
        }

        class WeatherService
        {
            public async Task<decimal> GetCurrentTemperature(string location)
            {
                // Simulate API call
                await Task.Delay(100);
                return location == "Göteborg" ? 11 : 25;
            }

            public async Task<string> GetForcecase(string location)
            {
                // Simulate API call
                await Task.Delay(100);
                return location == "Göteborg" ? "Rain" : "Sunny";
            }
        }
    }
}
