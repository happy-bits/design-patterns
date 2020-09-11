using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Visitor.Exports
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
                    client.ConfigureExporter("XML");
                    var result = client.ExportCircle(3, 4, 10);
                    Assert.AreEqual("<Circle><X>3</X><Y>4</Y><Radius>10</Radius></Circle>", result);
                }

                {
                    client.ConfigureExporter("Json");
                    var result = client.ExportCircle(3, 4, 10);
                    Assert.AreEqual("{x:3,y:4,radius:10}", result);
                }

            }
        }
    }
}
