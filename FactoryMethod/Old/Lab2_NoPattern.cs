using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Lab2_NoPattern
    {
        static readonly List<string> _actions = new List<string>();
        
        [TestMethod]
        public void Ex1()
        {
            var x = new ClientX();
            x.Run("production");

            var y = new ClientY();
            y.Run("production");

            CollectionAssert.AreEqual(new[] {
                "Enter ClientX",
                "Logger:ClientX",

                "Enter ClientY",
                "Logger:ClientY",
            }, _actions);
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
                ILoggable log;

                if (environment == "production")
                {
                    log = new Logger();
                }
                else
                {
                    log = new FakeLogger();
                }
                _actions.Add("Enter ClientX");
                log.Log("ClientX");

            }

        }

        class ClientY
        {

            internal void Run(string environment)
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
                _actions.Add("Enter ClientY");
                log.Log("ClientY");

            }

        }


    }
}
