
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Shapes2
    {
        [TestMethod]
        public void Ex1()
        {
            IEnumerable<Shape> shapes = Client.Run(4,"SC");

            CollectionAssert.AreEqual(new[] {
                "Square",
                "Circle",
                "Square",
                "Circle",
            }, shapes.Select(s => s.GetType().Name).ToArray());
        }

        [TestMethod]
        public void Ex2()
        {
            IEnumerable<Shape> shapes = Client.Run(6, "TTC");

            CollectionAssert.AreEqual(new[] {
                "Triangle",
                "Triangle",
                "Circle",
                "Triangle",
                "Triangle",
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
        class Triangle : Shape
        {
        }

        // Exercise: create the code below (Client and Factory)

        class Client
        {
            internal static IEnumerable<Shape> Run(int num, string factoryname)
            {
                ShapeFactory factory = SelectFactoryByName(factoryname);

                    //factoryname == "SC" ? new SquareCircleFactory() :
                    //factoryname == "TTC" ? (ShapeFactory) new TriangleTriangleCircleFactory() :
                    //throw new ArgumentException();

                var result = new List<Shape>();

                for (int i = 0; i < num; i++)
                {
                    result.Add(factory.GetShape());
                }
                return result;
            }

            private static ShapeFactory SelectFactoryByName(string factoryname) => factoryname switch
            {
                "SC" => new SquareCircleFactory(),
                "TTC" => new TriangleTriangleCircleFactory(),
                _ => throw new ArgumentException()
            };
        }

        abstract class ShapeFactory
        {
            protected int _counter = 0;

            public Shape GetShape()
            {
                _counter++;
                return CreateShape();
            }

            // Factory method

            abstract protected Shape CreateShape();
        }

        class SquareCircleFactory : ShapeFactory
        {
            private static bool IsEven(int number) => number % 2 == 0;

            protected override Shape CreateShape()
            {
                if (IsEven(_counter))
                    return new Circle();
                return new Square();
            }
        }

        class TriangleTriangleCircleFactory : ShapeFactory
        {
            private static bool DividableByThree(int number) => number % 3 == 0;

            protected override Shape CreateShape()
            {
                if (DividableByThree(_counter))
                    return new Circle();
                return new Triangle();
            }
        }
    }
}
