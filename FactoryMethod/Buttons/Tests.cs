using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Template.Buttons
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
                var result = client.DoStuff();

                CollectionAssert.AreEqual(new[] {
                    "Render Windows Button",
                    "Button clicked",
                    "Hide Dialog",
                    "Hide Button"

                }, result.ToArray());

            }
        }
    }
}
