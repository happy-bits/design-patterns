using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.TemplateMethod.Distances
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
                {
                    var result = client.License("   ABC     123     ");
                    Assert.AreEqual(Result.Success, result);
                }
                {
                    var result = client.License("ABC123");
                    Assert.AreEqual(Result.Invalid, result);
                }
                {
                    var result = client.License("AAA 666");
                    Assert.AreEqual(Result.SaveError, result);
                }
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {
                {
                    var result = client.Product("   123456     ");
                    Assert.AreEqual(Result.Success, result);
                }
                {
                    var result = client.Product("   1234 56     ");
                    Assert.AreEqual(Result.Invalid, result);
                }
                {
                    var result = client.Product("   666666     ");
                    Assert.AreEqual(Result.SaveError, result);
                }
            }
        }
    }
    public enum Result
    {
        Invalid, Success, SaveError
    }
}
