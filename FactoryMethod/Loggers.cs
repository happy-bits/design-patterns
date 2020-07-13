/*
 
FACTORY METHOD - design pattern (own example)

 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Loggers
    {
        // Comment: Demo2 looks good. Same feedback on Theory if you wanted but it's fine as-is.

        // (this field has nothing to do with the class Log below)
        static readonly List<string> _log = new List<string>();

        [TestMethod]
        public void Ex1()
        {
            var c = new Client();

            c.ClientCode(new DetailedLoggerCreator());
            c.ClientCode(new SimpleLoggerCreator());

            CollectionAssert.AreEqual(new[] {
                "Detailed logger writes info message 'MessageA'",
                "Detailed logger writes info message 'MessageB'",

                "Simple logger writes info message 'MessageA'",
                "Simple logger writes info message 'MessageB'",
            }, _log);
        }

        // "Creator"
        abstract class LoggerCreator
        {
            private Log _log;

            public abstract Log CreateLogger();

            public bool IsActive { get; set; }

            public void Info(string message)
            {
                if (!IsActive)
                    return;

                _log ??= CreateLogger();    // (compound assignment)

                _log.Info(message);
            }

        }

        // "Concrete Creator" 
        class DetailedLoggerCreator : LoggerCreator
        {
            public override Log CreateLogger()
            {
                return new DetailedLogger();
            }
        }

        // "Concrete Creator" 
        class SimpleLoggerCreator : LoggerCreator
        {
            public override Log CreateLogger()
            {
                return new SimpleLogger();
            }
        }

        abstract class Log
        {
            public abstract void Info(string message);
        }

        // "Concrete Product"
        class DetailedLogger : Log
        {
            public override void Info(string message)
            {
                _log.Add($"Detailed logger writes info message '{message}'");
            }
        }

        // "Concrete Product"
        class SimpleLogger : Log
        {
            public override void Info(string message)
            {
                _log.Add($"Simple logger writes info message '{message}'");
            }
        }

        // "Client"
        class Client
        {
            public void ClientCode(LoggerCreator creator)
            {
                creator.IsActive = true;
                creator.Info("MessageA");
                creator.Info("MessageB");

                creator.IsActive = false;
                creator.Info("MessageC");
            }
        }
    }
}
