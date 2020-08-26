using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Memento.Photoshop
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new Before2.Client(), 
            new After.Client() 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                client.DoStuff();
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
        public override string ToString()
        {
            return $"Dot({X},{Y})";
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

        public override string ToString()
        {
            return $"Circle({X},{Y},{Radius})";
        }
    }

}
