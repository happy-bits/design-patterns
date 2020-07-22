
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Shapes2_NoPattern
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
            private static bool IsEven(int number) => number % 2 == 0;
            private static bool DividableByThree(int number) => number % 3 == 0;

            internal static IEnumerable<Shape> Run(int num, string factoryname)
            {
                var result = new List<Shape>();

                switch (factoryname)
                {
                    case "SC":

                        for (int i = 1; i <= num; i++)
                        {

                            if (IsEven(i))
                                result.Add(new Circle());
                            else
                                result.Add(new Square());
                        }
                        return result;
                    case "TTC":

                        for (int i = 1; i <= num; i++)
                        {

                            if (DividableByThree(i))
                                result.Add(new Circle());
                            else
                                result.Add(new Triangle());
                        }
                        return result;
                    default: throw new ArgumentException();
                }

            }

        }
    }
}
