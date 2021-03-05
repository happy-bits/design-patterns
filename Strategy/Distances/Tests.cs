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

                AssertEqualCollection(new string[]{
                    "Distance between p1 and p2: 7",  // 3+4 = 7
                    "Distance between p1 and p2: 5"   // Sqrt(9+16) = 5
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
