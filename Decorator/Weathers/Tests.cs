using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator.Weathers
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] {
//            new Before.Cli(),
            new After.Client(),
        };


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
            foreach (var client in AllClients())
            {
                var ws = client.GetWeatherService();

                Temperature temperature = await ws.GetCurrentTemperature("Göteborg");
                Assert.AreEqual(11, temperature.Value);

                CollectionAssert.AreEqual(new[] {
                "GetCurrentTemperatur called",
                "GetCurrentTemperatur ended"
            }, client.Events.ToArray());
            }
        }

        [TestMethod]
        public async Task should_be_rainy_in_Göteborg()
        {
            foreach (var client in AllClients())
            {
                var ws = client.GetWeatherService();

                Forecast forecast = await ws.GetForcecase("Göteborg");
                Assert.AreEqual("Rain", forecast.Message);

                CollectionAssert.AreEqual(new[] {
                "GetForcecase called",
                "GetForcecase ended"
            }, client.Events.ToArray());
            }
        }

        [TestMethod]
        public async Task should_get_value_from_cache_two_times()
        {
            foreach (var client in AllClients())
            {
                var ws = client.GetWeatherService();

                Assert.AreEqual(0, client.ValueFromCache);

                await ws.GetCurrentTemperature("Göteborg");
                Assert.AreEqual(0, client.ValueFromCache);

                await ws.GetCurrentTemperature("Göteborg");
                Assert.AreEqual(1, client.ValueFromCache);

                await ws.GetCurrentTemperature("XYZ");
                Assert.AreEqual(1, client.ValueFromCache);
            }
        }

        [TestMethod]
        public async Task should_get_value_from_cache_one_time()
        {
            foreach (var client in AllClients())
            {
                var ws = client.GetWeatherService();

                Assert.AreEqual(0, client.ValueFromCache);

                await ws.GetForcecase("Göteborg");
                Assert.AreEqual(0, client.ValueFromCache);

                await ws.GetForcecase("Göteborg");
                Assert.AreEqual(1, client.ValueFromCache);
            }
        }
    }
}
