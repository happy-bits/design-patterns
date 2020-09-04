﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static DesignPatterns.TestUtilities;

namespace DesignPatterns.Builder.Cars
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
                client.ManualBuildTest();
                client.CarBuilderTest();
            }
        }
    }
}