

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Calculators
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

        class IocContainer
        {
            private ILoggable _logger;
            private ICalculator _calculator;

            public IocContainer(string environment)
            {
                var validValues = new[] { "production", "development" };

                if (!validValues.Contains(environment))
                    throw new ArgumentException();

                Environment = environment;
            }

            public static IocContainer Instance;

            public static void Setup(string environment)
            {
                Instance = new IocContainer(environment);
            }

            public string Environment { get; }

            internal ILoggable GetLogger()
            {
                if (Instance == null)
                    throw new InvalidOperationException();

                if (_logger == null)
                {
                    if (Environment == "production")
                    {
                        _logger = new Logger();
                    }
                    else
                    {
                        _logger = new AlternativeLogger();
                    }
                }
                return _logger;
            }

            internal ICalculator GetCalculator()
            {
                if (Instance == null)
                    throw new InvalidOperationException();

                if (_calculator == null)
                {
                    if (Environment == "production")
                    {
                        _calculator = new Calculator(GetLogger());
                    }
                    else
                    {
                        _calculator = new AlternativeCalculator(GetLogger());
                    }
                }
                return _calculator;
            }

        }

        class Client
        {
            public static void Run(string environment)
            {
                IocContainer.Setup(environment);

                var y = new ClientY();
                y.MethodA();
                y.MethodB();
            }
        }

        class ClientY
        {
            private readonly ICalculator _calculator = IocContainer.Instance.GetCalculator();

            public void MethodA()
            {
                _calculator.Add(3, 4);
            }

            public void MethodB()
            {
                _calculator.Add(3, 4);
            }
        }

    }
}
