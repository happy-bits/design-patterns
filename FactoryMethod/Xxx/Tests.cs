﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Template.Xxx
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new After.Client(),
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                var result = client.DoStuff();

                AssertEqualCollection(new[] {
                    "Ok button rendered as: <button>Ok</button>",
                    "Dialog closed",

                    "Ok button rendered as: [Ok]",
                    "Dialog closed",
                }, result);
            }
        }
    }
}
