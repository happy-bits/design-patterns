﻿/*

DECORATOR PATTERN

Good:

- Logging, Caching and Weather are separated. Easier to test the separate parts
- It's easy to combine them in other orders if you like
(own example)
 */
using DesignPatterns.Decorator.Weathers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DesignPatterns.Decorator.After
{
    class Client : IClient
    {
        private readonly List<string> _events = new List<string>();

        public IEnumerable<string> Events => _events;
        public int ValueFromCache { get; private set; }

        public IWeatherService GetWeatherService()
        {
            var service = new WeatherService();
            var cache = new CachingDecorator(service);
            var ws = new LoggingDecorator(cache);

            ws.Log += HandleLog;
            cache.GotValueFromCache += HandleGotValueFromCache;
            return ws;
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

    class CachingDecorator : IWeatherService
    {
        private readonly IWeatherService _inner;
        readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public EventHandler GotValueFromCache;

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
                GotValueFromCache?.Invoke(this, null);
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
                GotValueFromCache?.Invoke(this, null);
            }
            return _cache.Get<Forecast>(cachekey);
        }
    }

    class LoggingDecorator : IWeatherService
    {
        private readonly IWeatherService _inner;
        private readonly List<string> _weatherLogger = new List<string>();

        public EventHandler<string> Log;

        public LoggingDecorator(IWeatherService inner)
        {
            _inner = inner;
        }

        public Task<Temperature> GetCurrentTemperature(string location)
        {
            
            Log?.Invoke(this, "GetCurrentTemperatur called");
            var sw = Stopwatch.StartNew();

            var result = _inner.GetCurrentTemperature(location);

            sw.Stop();
            
            Log?.Invoke(this, "GetCurrentTemperatur ended");
            _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
            return result;
        }

        public Task<Forecast> GetForcecase(string location)
        {
            Log?.Invoke(this, "GetForcecase called");
            var sw = Stopwatch.StartNew();

            var result = _inner.GetForcecase(location);

            sw.Stop();
            
            Log?.Invoke(this, "GetForcecase ended");
            _weatherLogger.Add($"Time: {sw.ElapsedMilliseconds}ms");
            return result;
        }
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


}
