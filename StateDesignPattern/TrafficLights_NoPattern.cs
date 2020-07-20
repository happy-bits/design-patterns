﻿/*
 
 Own example with traffic lights

 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.StateDesignPattern
{
    [TestClass]
    public class TrafficLights_NoPattern
    {
        static Queue<string> _log = new Queue<string>();

        public enum Light
        {
            Red, RedYellow, Yellow, Green
        }
        [TestMethod]
        public void Ex1()
        {
            // "Client code"
            var light = new TrafficLight(Light.Red);
            Assert.AreEqual("Traffic is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state RedYellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Green", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚪",
                "⚫",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Yellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Traffic is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            Assert.AreEqual(0, _log.Count);
        }

        // "Context"
        internal class TrafficLight
        {
            private Light _light;

            public TrafficLight(Light light)
            {
                _light = light;

                _log.Enqueue($"Traffic is in state {_light}");
            }

            internal void Next()
            {
                _light = GetNextLight();
                _log.Enqueue($"Traffic is in state {_light}");
            }

            private Light GetNextLight() => _light switch
            {
                Light.Red => Light.RedYellow,
                Light.RedYellow => Light.Green,
                Light.Green => Light.Yellow,
                Light.Yellow => Light.Red,
                _ => throw new Exception()
            };

            internal string[] Render() => _light switch
            {
                Light.Red => new[] {
                    "⚫",
                    "⚪",
                    "⚪",
                },
                Light.RedYellow => new[] {
                "⚫",
                "⚫",
                "⚪",
                },
                Light.Green => new[] {
                "⚪",
                "⚪",
                "⚫",
                },
                Light.Yellow => new[] {
                "⚪",
                "⚫",
                "⚪",
                },  
                _ => throw new Exception()
            };
        }

    }
}
