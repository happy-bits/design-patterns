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
                "Enter ClientX",
                "Logger:ClientX",

                "Enter ClientY",
                "Logger:ClientY",
            }, _actions);
        }

        class LoggerFactory
        {
            private LoggerFactory()
            {

            }
            internal static ILoggable GetLogger(string environment)
            {
                ILoggable log;

                if (environment == "production")
                {
                    log = new Logger();
                }
                else
                {
                    log = new FakeLogger();
                }
                return log;
            }
        }
        interface ILoggable
        {
            void Log(string s);
        }

        class Logger : ILoggable
        {
            public void Log(string s)
            {
                _actions.Add($"Logger:{s}");
            }
        }

        class FakeLogger : ILoggable
        {
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
                _actions.Add("Enter ClientX");
                log.Log("ClientX");
            }

        }

        class ClientY
        {

            internal void Run(string environment)
            {
                ILoggable log = LoggerFactory.GetLogger(environment);
                _actions.Add("Enter ClientY");
                log.Log("ClientY");

            }

        }


    }
}
