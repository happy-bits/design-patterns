using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 Chain of command: a message should be handled in a specific order.
 
 Here: authorization. Rules of type Allow or Deny is chained together
 
*/
namespace DesignPatterns.ChainOfResponsibility
{
    [TestClass]
    public class Authentication
    {

        enum AuthResult { Allow, Deny }

        abstract class Rule
        {
            public Rule Next;
            public abstract AuthResult Handle(Request request);
        }

        class Allow : Rule
        {
            public Func<Request, bool> Condition;
            public override AuthResult Handle(Request r) => Condition.Invoke(r) ? AuthResult.Allow : Next.Handle(r);
        }

        class Deny : Rule
        {
            public Func<Request, bool> Condition;
            public override AuthResult Handle(Request r) => Condition.Invoke(r) ? AuthResult.Deny : Next.Handle(r);
        }

        class Request
        {
            public User User { get; set; }
        }

        class User
        {
            public string[] Roles { get; set; }
            public bool IsAuthenticated { get; set; }
            public string Name { get; set; }

            internal bool IsInRole(string rolename)
            {
                return Roles.Contains(rolename);
            }
        }

        /* 
           Create a rule that gives Allow if user is:

                authenticated && (administrator || name is "bobo")
        */

        Rule chain = new Deny
        {
            Condition = r => !r.User.IsAuthenticated,
            Next = new Allow
            {
                Condition = r => r.User.IsInRole("Administrator"),
                Next = new Allow
                {
                    Condition = r => r.User.Name == "bobo",
                    Next = new Deny
                    {
                        Condition = r => true
                    }
                }
            }
        };

        [TestMethod]
        public void should_be_denied_if_not_authenticated()
        {
            var user = new User
            {
                IsAuthenticated = false,
                Roles = new[] { "Editor", "Administrator" },
                Name = "bobo"
            };

            Assert.AreEqual(AuthResult.Deny, chain.Handle(new Request { User = user }));
        }

        [TestMethod]
        public void should_deny_if_not_in_adminrole_and_not_bobo()
        {
            var user = new User
            {
                IsAuthenticated = true,
                Roles = new[] { "Editor" },
                Name = "lucy",
            };

            Assert.AreEqual(AuthResult.Deny, chain.Handle(new Request { User = user }));
        }

        [TestMethod]
        public void should_allow_authenicated_and_bobo()
        {
            var user = new User
            {
                IsAuthenticated = true,
                Roles = new[] { "Editor" },
                Name = "bobo",
            };

            Assert.AreEqual(AuthResult.Allow, chain.Handle(new Request { User = user }));
        }

        [TestMethod]
        public void should_allow_authenicated_and_administrator()
        {
            var user = new User
            {
                IsAuthenticated = true,
                Roles = new[] { "Administrator" },
                Name = "lucy",
            };

            Assert.AreEqual(AuthResult.Allow, chain.Handle(new Request { User = user }));
        }
    }

}
