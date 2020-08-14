using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Shapes_NoPatternOld
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

        // Exercise: create the "Client"-code below

        class Client
        {
            private static bool IsEven(int number) => number % 2 == 0;

            internal static IEnumerable<Shape> Run(int num)
            {
                var result = new List<Shape>();

                for (int i = 1; i <= num; i++) {

                    if (IsEven(i))
                        result.Add(new Circle());
                    else
                        result.Add(new Square());
                }

                return result;

            }
        }
    }
}
