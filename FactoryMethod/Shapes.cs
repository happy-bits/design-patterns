// Akademiskt

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class ShapesOld
    {
        [TestMethod]
        public void Ex1()
        {
            IEnumerable<Shape> shapes = Client.Run(4);

            CollectionAssert.AreEqual(new[] {
                "Square",
                "Circle",
                "Square",
                "Circle",
            }, shapes.Select(s => s.GetType().Name).ToArray());
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

        // Exercise: create the code below (Client and Factory)
        class Client
        {
            internal static IEnumerable<Shape> Run(int num)
            {
                var factory = new ShapeFactory();

                var result = new List<Shape>();

                for (int i = 0; i < num; i++)
                {
                    result.Add(factory.CreateShape());
                }
                return result;
            }
        }

        class ShapeFactory
        {
            private int _counter = 0;

            internal Shape CreateShape()
            {
                _counter++;

                if (IsEven(_counter))
                    return new Circle();
                return new Square();
            }

            private static bool IsEven(int number) => number % 2 == 0;
        }
    }
}
