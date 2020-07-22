
// todo: Lägg till mer logik i LogCreator så det blir mer intressant

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Logz_NoPattern
    {
        [TestMethod]
        public void Ex1()
        {
            Client.Run("dev");

            CollectionAssert.AreEqual(new[] {
                "Trace:A (DevelopmentLogger)",
                "Warning:B (DevelopmentLogger)",
            }, _events);

            _events.Clear();

            Client.Run("prod");

            CollectionAssert.AreEqual(new[] {
                "Warning:B (ProductionLogger)",
            }, _events);
        }

        static List<string> _events = new List<string>();

        abstract class Logger
        {
            public abstract void Trace(string message);
            public abstract void Warning(string message);
        }

        class DevelopmentLogger : Logger
        {
            public override void Trace(string message)
            {
                _events.Add($"Trace:{message} (DevelopmentLogger)");
            }

            public override void Warning(string message)
            {
                _events.Add($"Warning:{message} (DevelopmentLogger)");
            }
        }
        class ProductionLogger : Logger
        {
            public override void Trace(string message)
            {
                // Nothing traced in production
            }

            public override void Warning(string message)
            {
                _events.Add($"Warning:{message} (ProductionLogger)");
            }
        }


        // Exercise: create the code below (Client and LogCreator)

        class LogCreator
        {
            private readonly string _factoryname;

            public LogCreator(string factoryname)
            {
                _factoryname = factoryname;
            }
            internal Logger GetLogger()
            {
                switch (_factoryname)
                {
                    case "dev":
                        return new DevelopmentLogger();

                    case "prod":

                        return new ProductionLogger();

                    default: throw new ArgumentException();
                }
            }
        }

        class Client
        {
            internal static void Run(string environment)
            {
                Logger logger = new LogCreator(environment).GetLogger();
                logger.Trace("A");
                logger.Warning("B");
            }

        }
    }
}
