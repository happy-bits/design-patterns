using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod.Xxx
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
                    var result = client.RenderAlphaMenu();
                    Assert.AreEqual("A) BANANA\nB) APPLE", result);
                }
                {
                    var result = client.RenderNumericMenu();
                    Assert.AreEqual("1) BANANA\n2) APPLE", result);
                }

            }
        }
    }
}
