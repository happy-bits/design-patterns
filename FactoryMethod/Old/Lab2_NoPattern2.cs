
// todo: vad är problemet med denna lösning?

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Lab2_NoPattern2
    {
        static readonly List<string> _actions = new List<string>();
        
        [TestMethod]
        public void Ex1()
        {
            string environment = "production";
            ILoggable log;
            if (environment == "production")
            {
                log = new Logger();
            }
            else
            {
                log = new FakeLogger();
            }

            var x = new ClientX();
            x.Run(log);

            var y = new ClientY();
            y.Run(log);

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
            internal void Run(ILoggable log)
            {
                _actions.Add("ClientX");
                log.Log("1");
                log.Log("2");
            }
        }

        class ClientY
        {
            internal void Run(ILoggable log)
            {
                _actions.Add("ClientY");
                log.Log("3");
                log.Log("4");
            }
        }
    }
}
