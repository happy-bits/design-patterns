
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Adapter.Pegs
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new After.Client(), 
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

    class SquarePeg
    {

        public SquarePeg(double width)
        {
            Width = width;
        }

        public double Width { get; }
    }

    class RoundPeg
    {
        public RoundPeg(double radius)
        {
            Radius = radius;
        }

        public virtual double Radius { get; } // behöver vara "virtual"
    }
}
