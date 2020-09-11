using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Visitor.Exports
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
                var circle = client.CreateCircle(3, 4, 10);
                var dot = client.CreateDot(5, 6);

                {
                    client.ConfigureExporter("XML");

                    var result = client.ExportShapes(circle, dot);

                    CollectionAssert.AreEqual(new[] {
                        "<Circle><X>3</X><Y>4</Y><Radius>10</Radius></Circle>",
                        "<Dot><X>5</X><Y>6</Y></Dot>"
                    }, result.ToArray());
                }

                {
                    client.ConfigureExporter("Json");

                    var result = client.ExportShapes(circle, dot);

                    CollectionAssert.AreEqual(new[] {
                        "{x:3,y:4,radius:10}",
                        "{x:5,y:6}"
                    }, result.ToArray());
                }
            }
        }
    }
}
