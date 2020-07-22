
// todo: vad är problemet?

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Calculators_NoPattern
    {
        static readonly List<string> _actions = new List<string>();
        
        [TestMethod]
        public void Ex1()
        {
            
            Client.Run("production");
            
            CollectionAssert.AreEqual(new[] {
                    "New Logger",
                    "New Calculator",

                    "Calculator Add",
                    "Logger:1",
                    "Logger:2",

                    "Calculator Add",
                    "Logger:1",
                    "Logger:2",
                }, _actions);

            _actions.Clear();

            Client.Run("development");

            CollectionAssert.AreEqual(new[] {
                    "New AlternativeLogger",
                    "New AlternativeCalculator",

                    "AlternativeCalculator Add",
                    "AlternativeLogger:3",
                    "AlternativeLogger:4",

                    "AlternativeCalculator Add",
                    "AlternativeLogger:3",
                    "AlternativeLogger:4",
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

        class AlternativeLogger : ILoggable
        {
            public AlternativeLogger()
            {
                _actions.Add("New AlternativeLogger");
            }
            public void Log(string s)
            {
                _actions.Add($"AlternativeLogger:{s}");
            }
        }

        interface ICalculator
        {
            double Add(double a, double b);
        }

        class Calculator : ICalculator
        {
            private readonly ILoggable _log;
            
            public Calculator(ILoggable log)
            {
                _actions.Add("New Calculator");
                _log = log;
            }

            public double Add(double a, double b)
            {
                _actions.Add("Calculator Add");
                _log.Log("1");
                _log.Log("2");
                return a + b;
            }
        }

        class AlternativeCalculator : ICalculator
        {
            private readonly ILoggable _log;

            public AlternativeCalculator(ILoggable log)
            {
                _actions.Add("New AlternativeCalculator");
                _log = log;
            }

            public double Add(double a, double b)
            {
                _actions.Add("AlternativeCalculator Add");
                _log.Log("3");
                _log.Log("4");
                return a + b;
            }
        }


        class Client
        {
            public static void Run(string environment)
            {
                ILoggable log;
                ICalculator calculator;

                if (environment == "production")
                {
                    log = new Logger();
                    calculator = new Calculator(log);
                }
                else
                {
                    log = new AlternativeLogger();
                    calculator = new AlternativeCalculator(log);
                }
                var result1 = calculator.Add(3, 4);
                var result2 = calculator.Add(5, 10);

                Assert.AreEqual(7, result1);
                Assert.AreEqual(15, result2);

            }
        }

    }
}
