using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Mediator.Forms
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
                var result = client.EnterTextAndClickSubmit("123", "abc");

                CollectionAssert.AreEqual(new []{
                    "Form submitted",
                    "text1=123 text2=abc"
                }, result.ToArray());
            }
        }

        [TestMethod]
        public void Ex2()
        {
            foreach (var client in AllClients())
            {
                var result = client.EnterTextAndClickSubmit("123", "aaaaaaaaaaaaaa");

                CollectionAssert.AreEqual(new[]{
                    "Tried submit form, but not all fields are valid"
                }, result.ToArray());
            }
        }


        [TestMethod]
        public void Ex3()
        {
            foreach (var client in AllClients())
            {
                var result = client.EnterTextAndClearForm("123", "aaaaaaaaaaaaaa");

                CollectionAssert.AreEqual(new[]{
                    "Form cleared",
                    "text1= text2="
                }, result.ToArray());
            }
        }
    }
}
