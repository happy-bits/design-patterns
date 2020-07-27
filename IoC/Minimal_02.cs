/*
   Small adjustments of: https://nearsoft.com/blog/writing-a-minimal-ioc-container-in-c/

   If you demand a IGreeter you might get a SwedishGreeter which will auto resolve FileLogger
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.IoC
{
    [TestClass]
    public class Minimal_02
    {
        [TestMethod]
        public void swedish()
        {
            var container = new MinimalContainer();
            container.Register<IGreeter, SwedishGreeter>();
            container.Register<ILogger, FileLogger>();

            var greeter = container.Create<IGreeter>();
            var result = greeter.SayHello();

            Assert.AreEqual("Hej!", result);

            var result2 = greeter.GetLog();

            Assert.AreEqual("*Log to file*", result2);
        }

        [TestMethod]
        public void english()
        {
            var container = new MinimalContainer();
            container.Register<IGreeter, EnglishGreeter>();
            container.Register<ILogger, FileLogger>();

            var greeter = container.Create<IGreeter>();
            var result = greeter.SayHello();

            Assert.AreEqual("Hello!", result);
        }

        interface IGreeter
        {
            string SayHello();
            string GetLog();
        }

        interface ILogger
        {
            string Log();
        }

        interface IOtherInterface
        {
        }

        class FileLogger : ILogger
        {
            public string Log()
            {
                return "Log to file";
            }
        }
        class SwedishGreeter : IGreeter
        {
            private readonly ILogger _logger;

            public SwedishGreeter(ILogger logger)
            {
                _logger = logger;
            }

            public string GetLog()
            {
                return $"*{_logger.Log()}*";
            }

            public string SayHello()
            {
                return "Hej!";
            }
        }

        class EnglishGreeter : IGreeter
        {
            public string GetLog()
            {
                throw new NotImplementedException();
            }

            public string SayHello()
            {
                return "Hello!";
            }
        }

        public class MinimalContainer
        {
            private readonly Dictionary<Type, Type> types = new Dictionary<Type, Type>();

            public void Register<TInterface, TImplementation>() where TImplementation : TInterface
            {
                types[typeof(TInterface)] = typeof(TImplementation);
            }

            public TInterface Create<TInterface>()
            {
                return (TInterface)Create(typeof(TInterface));
            }

            private object Create(Type type)
            {
                //Find a default constructor using reflection
                var concreteType = types[type];
                var defaultConstructor = concreteType.GetConstructors()[0];

                //Verify if the default constructor requires params
                var defaultParams = defaultConstructor.GetParameters();

                //Instantiate all constructor parameters using recursion
                var parameters = defaultParams.Select(param => Create(param.ParameterType)).ToArray();

                return defaultConstructor.Invoke(parameters);
            }

        }
    }
}
