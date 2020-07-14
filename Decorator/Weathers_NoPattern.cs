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
            var result = await ws.GetCurrentTemperature("Göteborg");
            Assert.AreEqual(11, result.Value);
        }

        [TestMethod]
        public async Task should_be_rainy_in_Göteborg()
        {
            var ws = new WeatherService();
            var result = await ws.GetForcecase("Göteborg");
            Assert.AreEqual("Rain", result.Message);
        }

        class WeatherService
        {
            public async Task<Temperature> GetCurrentTemperature(string location)
            {
                // Simulate API call
                await Task.Delay(100);
                return location == "Göteborg" ? new Temperature(11) : new Temperature(25);
            }

            public async Task<Forecast> GetForcecase(string location)
            {
                // Simulate API call
                await Task.Delay(100);
                return location == "Göteborg" ? new Forecast("Rain") : new Forecast("Sunny");
            }
        }
    }
}
