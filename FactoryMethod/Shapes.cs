// Akademiskt

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Shapes
    {
        [TestMethod]
        public void Ex1()
        {
            var factory = new ShapeFactory();

            var result = new List<Shape>();

            result.Add(factory.CreateShape());
            result.Add(factory.CreateShape());
            result.Add(factory.CreateShape());
            result.Add(factory.CreateShape());

            CollectionAssert.AreEqual(new[] {
                "Square",
                "Circle",
                "Square",
                "Circle",
            }, result.Select(s => s.GetType().Name).ToArray());
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

        private bool IsEven(int number) => number % 2 == 0;
    }

    abstract class Shape
    {

    }
    class Circle: Shape
    {

    }
    class Square: Shape
    {

    }
}
