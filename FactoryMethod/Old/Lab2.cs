using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Lab2
    {
        static readonly List<string> _actions = new List<string>();
        
        [TestMethod]
        public void Ex1()
        {
            string environment = "production";
            var x = new ClientX();
            x.Run(environment);

            var y = new ClientY();
            y.Run(environment);

            CollectionAssert.AreEqual(new[] {
                "New Logger",

                "ClientX",
                "Logger:1",
                "Logger:2",

                "ClientY",
                "Logger:3",
                "Logger:4",
            }, _actions);
        }

        class LoggerFactory
        {
            private static ILoggable _log;

            internal static ILoggable GetLogger(string environment)
            {
                if (_log != null)
                    return _log;

                if (environment == "production")
                {
                    _log = new Logger();
                }
                else
                {
                    _log = new FakeLogger();
                }
                return _log;
            }
        }
        interface ILoggable
        {
            void Log(string s);
        }

        class Logger : ILoggable
        {
            public Logger()
            {
                _actions.Add("New Logger");
            }
            public void Log(string s)
            {
                _actions.Add($"Logger:{s}");
            }
        }

        class FakeLogger : ILoggable
        {
            public FakeLogger()
            {
                _actions.Add("New FakeLogger");
            }
            public void Log(string s)
            {
                _actions.Add($"FakeLogger:{s}");
            }
        }

        class ClientX
        {

            internal void Run(string environment)
            {
                ILoggable log = LoggerFactory.GetLogger(environment);
                _actions.Add("ClientX");
                log.Log("1");
                log.Log("2");
            }

        }

        class ClientY
        {

            internal void Run(string environment)
            {
                ILoggable log = LoggerFactory.GetLogger(environment);
                _actions.Add("ClientY");
                log.Log("3");
                log.Log("4");

            }

        }


    }
}
