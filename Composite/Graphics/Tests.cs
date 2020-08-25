using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Composite.Graphics
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
        //    new Before.Client(), 
            new After.Client() 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var result = client.DrawStuff();

                AssertEqualCollection(new[] {
                    "Drawing dot on position (3,4)",
                    "Drawing circle with radius 100 on position (20,30)",

                    "Drawing dot on position (500,600)",
                    "Drawing circle with radius 100 on position (500,600)",
                    "Drawing circle with radius 77 on position (500,600)",
                    "Drawing dot on position (500,600)",

                    }, result);
            }
        }
    }
}
