using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Memento.Distances
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            //new Before.Client(), 
            new After.Client() 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                client.Calculate();
            }
        }


    }


    abstract class Graphic
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
    }


    class Dot : Graphic
    {
        public Dot(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    class Circle : Graphic
    {
        public Circle(double x, double y, double radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public double Radius { get; }

    }

}
