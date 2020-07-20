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
        public enum Light
        {
            Red, RedYellow, Yellow, Green
        }
        
        [TestMethod]
        public void Ex1()
        {
            var light = new TrafficLight(Material.LED, Light.Red);

            Assert.AreEqual("Rött", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Rött och gult", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Grönt", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚪",
                "⚫",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Gult", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚪",
                "⚫",
                "⚪",
            }, light.Render());

            light.Next();
            Assert.AreEqual("Rött", light.Describe);
            CollectionAssert.AreEqual(new[] {
                "⚫",
                "⚪",
                "⚪",
            }, light.Render());

            Assert.AreEqual("LED", light.Material.ToString());
        }

        public enum Material
        {
            Halogen, LED
        }
        
        public class TrafficLight
        {
            private Light _light;

            public TrafficLight(Material material, Light light)
            {
                _light = light;
                Material = material;
            }

            public Material Material { get; }

            public void Next()
            {
                _light = GetNextLight();
            }

            private Light GetNextLight() => _light switch
            {
                Light.Red => Light.RedYellow,
                Light.RedYellow => Light.Green,
                Light.Green => Light.Yellow,
                Light.Yellow => Light.Red,
                _ => throw new Exception()   // nackdel
            };

            public string Describe => _light switch
            {
                Light.Red => "Rött",
                Light.RedYellow => "Rött och gult",
                Light.Green => "Grönt",
                Light.Yellow => "Gult",
                _ => throw new Exception()    // nackdel
            };

            public string[] Render() => _light switch
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
                _ => throw new Exception()    // nackdel
            };
        }

    }
}
