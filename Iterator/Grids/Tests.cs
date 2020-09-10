using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Iterator.Grids
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
                {
                    var result = client.ForEachFromLeftToRight();

                    CollectionAssert.AreEqual(new[] {
                        "a", "b", "c", "d", "e", "f", "g", "h"
                    }, result);
                }
                {
                    var result = client.ForEachFromUpToDown();

                    CollectionAssert.AreEqual(new[] {
                        "a", "e", "b", "f", "c", "g", "d", "h"
                    }, result);
                }

            }
        }
    }
}
