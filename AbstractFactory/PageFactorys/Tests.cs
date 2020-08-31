using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.AbstractFactory.PageFactorys
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            //new Before.Client(), 
            new After.Client(),
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                {
                    client.SetFactory("Round");
                    var result = client.RenderPage("Header", "Lorem ipsum dolor");

                    CollectionAssert.AreEqual(new[] {
                        "╭────────╮",
                        "│ HEADER │",
                        "╰────────╯",
                        "Lorem ipsum dolor",
                    }
                    , result);
                }
                {
                    client.SetFactory("Html");
                    var result = client.RenderPage("Header", "Lorem ipsum dolor");

                    CollectionAssert.AreEqual(new[] {
                        "<h1>Header</h1>",
                        "<p>Lorem ipsum dolor</p>"
                    }
                    , result);
                }

                client.SetFactory("Html");
                var result2 = client.RenderPage("Header", "Lorem ipsum dolor");
            }
        }
    }
}
