using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Template.Requests2
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
                client.SetupChain();

                {
                    var request = new Request { 
                        User = new User 
                        { 
                            Roles = new[] { "Administrator" },
                        },
                        PageId=100
                    };
                    var response = client.Authenticate(request);
                    Assert.IsTrue(response.Authenticated);
                    Assert.AreEqual(PageType.Politics, response.PageType);
                }

                {
                    var request = new Request
                    {
                        User = new User
                        {
                            Roles = new[] { "AAAAA" }
                        },
                        PageId = 100
                    };
                    var response = client.Authenticate(request);
                    Assert.IsFalse(response.Authenticated);
                    Assert.AreEqual(PageType.Politics, response.PageType);
                }

                {
                    var request = new Request
                    {
                        User = new User
                        {
                            Name="bobo",
                            Roles = new[] { "AAAAA" }
                        },
                        PageId = 100
                    };
                    var response = client.Authenticate(request);
                    Assert.IsTrue(response.Authenticated);
                    Assert.AreEqual(PageType.Politics, response.PageType);
                }


                {
                    var request = new Request
                    {
                        User = new User
                        {
                            Roles = new[] { "PoliticsEditor" }
                        },
                        PageId = 100
                    };
                    var response = client.Authenticate(request);
                    Assert.IsTrue(response.Authenticated);
                    Assert.AreEqual(PageType.Politics, response.PageType);
                }
            }
        }
    }
    class Request
    {
        public User User { get; set; }
        public int PageId { get; set; }
    }

    class Response
    {
        public bool Authenticated { get; set; }
        public Request Request { get; set; }
        public PageType PageType { get; set; }
    }

    enum PageType
    {
        Unknown, Sports, News, Politics
    }

    class User
    {
        public string[] Roles { get; set; }
        public string Name { get; set; }

        public bool IsInRole(string rolename)
        {
            return Roles.Contains(rolename);
        }
    }
}
