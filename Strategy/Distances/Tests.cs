using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Strategy.Distances
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new After.Client() 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var p1 = new Point(10, 11);
                var p2 = new Point(13, 15);

                var result = client.CalculateManhattanThenBird(p1, p2);

                AssertEqualCollection(new double[]{
                    7,  // 3+4
                    5   // Sqrt(9+16)
                }, 
                result);  
            }
        }
    }

    struct Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; }
        public double Y { get; }
    }
}
