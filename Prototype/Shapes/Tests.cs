﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Prototype.Shapes
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] { 
            new Before1.Client(), 
            new Before2.Client(), 
            new Before3.Client(), 
            new After.Client(), 
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                client.DoStuff();
            }
        }
    }
}
