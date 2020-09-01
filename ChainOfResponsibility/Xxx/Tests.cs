using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Template.Xxx
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
                    var request = new Request { User = new User { 
                        Roles = new[] { "Administrator" } } };
                    var response = client.Authenticate(request);
                    Assert.IsTrue(response.Authenticated);
                }

                {
                    var request = new Request
                    {
                        User = new User
                        {
                            Roles = new[] { "AAAAA" }
                        }
                    };
                    var response = client.Authenticate(request);
                    Assert.IsFalse(response.Authenticated);
                }

                {
                    var request = new Request
                    {
                        User = new User
                        {
                            Name="bobo",
                            Roles = new[] { "AAAAA" }
                        }
                    };
                    var response = client.Authenticate(request);
                    Assert.IsTrue(response.Authenticated);
                }
            }
        }
    }
    class Request
    {
        public User User { get; set; }
    }

    class Response
    {
        public Response(bool authenticated)
        {
            Authenticated = authenticated;
        }

        public bool Authenticated { get; }
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
