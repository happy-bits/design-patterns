
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Shapes2_NoPattern_Refactor
    {
        [TestMethod]
        public void Ex1()
        {
            IEnumerable<Shape> shapes = Client.Run(4, "SC");

            CollectionAssert.AreEqual(new[] {
                "Square",
                "Circle",
                "Square",
                "Circle",
            }, ClassNames(shapes));
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

            }, ClassNames(shapes));
        }

        private static string[] ClassNames(IEnumerable<object> shapes) => shapes.Select(s => s.GetType().Name).ToArray();

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

        // Extra: testa de två fabrikerna

        [TestMethod]
        public void test_SquareCircleFactory()
        {
            var factory = new Creator("SC");

            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Square);
            Assert.IsTrue(factory.GetShape() is Circle);
        }

        [TestMethod]
        public void test_TriangleTriangleCircleFactory()
        {
            var factory = new Creator("TTC");

            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Triangle);
            Assert.IsTrue(factory.GetShape() is Circle);
            Assert.IsTrue(factory.GetShape() is Triangle);
        }

        // Exercise: create the code below (Client and Factory)

        // Nackdel: ex DividibleByThree behövs bara i fallet "TTC".

        // Nackdel: klassen behöver uppdateras och kommer växa när nya sorters fabriker behövs
        
        class Creator
        {
            private readonly string _factoryname;
            private int _counter = 0;

            private static bool IsEven(int number) => number % 2 == 0;
            private static bool DividableByThree(int number) => number % 3 == 0;

            public Creator(string factoryname)
            {
                _factoryname = factoryname;
            }
            internal Shape GetShape()
            {
                _counter++;
                switch (_factoryname)
                {
                    case "SC":

                        if (IsEven(_counter))
                            return new Circle();
                        else
                            return new Square();

                    case "TTC":

                        if (DividableByThree(_counter))
                            return new Circle();
                        else
                            return new Triangle();

                    default: throw new ArgumentException();
                }
            }
        }

        class Client
        {
            internal static IEnumerable<Shape> Run(int num, string factoryname)
            {
                var result = new List<Shape>();

                var creator = new Creator(factoryname);

                for (int i = 1; i <= num; i++)
                {
                    result.Add(creator.GetShape());
                }
                return result;
            }

        }
    }
}
