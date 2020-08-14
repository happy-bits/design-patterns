using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.FactoryMethod.Shapes
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { new Before.Client(), new After.Client() };

        [TestMethod]
        public void Ex1()
        {
            foreach(var client in AllClients())
            {
                IEnumerable<Shape> shapes = client.Run(4, "SquareCircle");

                CollectionAssert.AreEqual(new[] {
                    "Square",
                    "Circle",
                    "Square",
                    "Circle",
                }, ClassNames(shapes));
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {

                IEnumerable<Shape> shapes = client.Run(6, "TriangleTriangleCircle");

                CollectionAssert.AreEqual(new[] {
                "Triangle",
                "Triangle",
                "Circle",
                "Triangle",
                "Triangle",
                "Circle",

            }, ClassNames(shapes));
            }
        }
    }


    abstract class Shape
    {
    }
    class Circle : Shape
    {
    }
    class Square : Shape
    {
    }
    class Triangle : Shape
    {
    }
}
