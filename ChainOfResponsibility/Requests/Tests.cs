using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DesignPatterns.ChainOfResponsibility.Requests
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before.Client(), 
            new After.Client(), 
        };

        [TestMethod]
        public void admin_should_be_authenticated()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User
                    {
                        Roles = new[] { "Administrator" },
                    },
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsTrue(response.Authorized);
                Assert.IsTrue(response.DatabaseIsCalled);
            } 
        }

        [TestMethod]
        public void bobo_should_be_authenticated()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User
                    {
                        Name = "bobo",
                    },
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsTrue(response.Authorized);
                Assert.IsTrue(response.DatabaseIsCalled);
            }
        }

        [TestMethod]
        public void user_without_role_is_not_enough()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User(),
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsFalse(response.Authorized);
                Assert.IsFalse(response.DatabaseIsCalled); // note: database don't need to be called
            }
        }

        [TestMethod]
        public void just_Editor_is_not_enough()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User
                    {
                        Roles = new[] { "Editor" }
                    },
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsFalse(response.Authorized);
                Assert.IsTrue(response.DatabaseIsCalled); // note: database have to be called
            }
        }

        [TestMethod]
        public void SportEditor_is_not_enough()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User
                    {
                        Roles = new[] { "Editor", "SportEditor" }
                    },
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsFalse(response.Authorized);
                Assert.IsTrue(response.DatabaseIsCalled); // note: databas is called (we have to figure out type of page)
            }
        }

        [TestMethod]
        public void PoliticalEditor_is_enough()
        {
            foreach (var client in AllClients())
            {
                client.SetupChain();
                var request = new Request
                {
                    User = new User
                    {
                        Roles = new[] { "PoliticsEditor", "Editor" }
                    },
                    PageId = 100
                };
                var response = client.GetPage(request);
                Assert.IsTrue(response.Authorized);
                Assert.IsTrue(response.DatabaseIsCalled);
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
        public bool Authorized { get; set; }
        public Page Page { get; set; }
        public bool DatabaseIsCalled => Page != null;
    }

    enum PageType
    {
        Unknown, Sports, News, Politics
    }

    class User
    {
        public string[] Roles { get; set; } = new string[] { };
        public string Name { get; set; }

        public bool IsInRole(string rolename)
        {
            return Roles.Contains(rolename);
        }
    }


    class Page
    {
        public int Id { get; set; }
        public PageType PageType { get; set; }
        public string Content { get; set; }
    }

    class PageRepository
    {
        public Page GetPageById(int id)
        {
            // Simulation
            Thread.Sleep(10);
            if (id == 100)
            {
                return new Page
                {
                    PageType = PageType.Politics,
                    Content = "Bla bla"
                };
            }
            throw new Exception();
        }
    }
}
