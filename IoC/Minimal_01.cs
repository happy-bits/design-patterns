﻿/*
Inspired by:

https://nearsoft.com/blog/writing-a-minimal-ioc-container-in-c/


 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.IoC
{
    [TestClass]
    public class Minimal_01
    {
        [TestMethod]
        public void choose_SwedishGreeter_for_IGreeter()
        {
            var container = new MinimalContainer();
            container.Register<IGreeter, SwedishGreeter>();

            var greeter = container.Create<IGreeter>();
            var result = greeter.SayHello();

            Assert.AreEqual("Hej!", result);
        }

        [TestMethod]
        public void choose_EnglishGreeter_for_IGreeter()
        {
            var container = new MinimalContainer();
            container.Register<IGreeter, EnglishGreeter>();

            var greeter = container.Create<IGreeter>();
            var result = greeter.SayHello();

            Assert.AreEqual("Hello!", result);
        }

        private void test()
        {
            var container = new MinimalContainer();
            //container.Register<IOtherInterface, EnglishGreeter>(); <--- doesn't work (good)
        }

        interface IGreeter
        {
            string SayHello();
        }

        interface IOtherInterface
        {
        }

        class SwedishGreeter : IGreeter
        {
            public string SayHello()
            {
                return "Hej!";
            }
        }

        class EnglishGreeter : IGreeter
        {
            public string SayHello()
            {
                return "Hello!";
            }
        }

        class MinimalContainer
        {
            readonly Dictionary<Type, Type> _types = new Dictionary<Type, Type>();

            // Hämta klass utifrån interfacet "T". 
            // Returnera instans av klassen

            public T Create<T>()
            {
                var concreteType =  _types[typeof(T)];
                var defaultConstructor = concreteType.GetConstructors()[0];
                return (T)defaultConstructor.Invoke(null); 
            }
            
            // Lägg till typerna i "_types"
            public void Register<TInterface, TImplementation>() where TImplementation: TInterface  
            {
                _types[typeof(TInterface)] = typeof(TImplementation);
            }


        }
    }
}
