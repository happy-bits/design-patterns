/*
 
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
            var light = new TrafficLight(Material.LED, Light.Red);

            Assert.AreEqual("Light is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Light is in state RedYellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Light is in state Green", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚪",
                "⚫",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Light is in state Yellow", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Light is in state Red", _log.Dequeue());
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            Assert.AreEqual("LED", light.Material.ToString());

            Assert.AreEqual(0, _log.Count);
        }

        public enum Material
        {
            Halogen, LED
        }

        // "Context"
        internal class TrafficLight
        {
            private Light _light;

            public TrafficLight(Material material, Light light)
            {
                _light = light;
                Material = material;
                _log.Enqueue($"Light is in state {_light}");
            }

            public Material Material { get; }

            internal void Next()
            {
                _light = GetNextLight();
                _log.Enqueue($"Light is in state {_light}");
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
