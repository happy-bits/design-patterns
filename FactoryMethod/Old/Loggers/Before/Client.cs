using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns.Template.Loggers.Before
{
    class Client : IClient
    {
        private static List<string> _events = new List<string>();

        public void DoStuff()
        {
            // throw new NotImplementedException();

            var c = new ConsoleLogProvider();
            var f = new FileLogProvider("C:\\log.txt");

            var logger = new Logger(c, f);

            logger.Info("Some info");
            logger.Warning("A warning");

            CollectionAssert.AreEqual(new[] {

                "Log information to Console: Some info",
                "Log information to file 'C:\\log.txt': Some info",

                "Log warning to Console: A warning",
                "Log warning to file 'C:\\log.txt': A warning",

            }, _events);

        }

        interface ILogger
        {
            void Info(string message);
            void Warning(string message);
        }

        class Logger: ILogger
        {
            private readonly LogProvider[] _logProvider;

            public Logger(params LogProvider[] logProvider)
            {
                _logProvider = logProvider;
            }

            public void Info(string message)
            {
                foreach(var provider in _logProvider)
                {
                    provider.Info(message);
                }
            }

            public void Warning(string message)
            {
                foreach (var provider in _logProvider)
                {
                    provider.Warning(message);
                }
            }
        }

        abstract class LogProvider : ILogger
        {
            public abstract void Info(string message);
            public abstract void Warning(string message);
        }

        class ConsoleLogProvider : LogProvider
        {
            public override void Info(string message)
            {
                _events.Add($"Log information to Console: {message}");
            }

            public override void Warning(string message)
            {
                _events.Add($"Log warning to Console: {message}");
            }
        }

        class FileLogProvider : LogProvider
        {
            private readonly string _logfilename;

            public FileLogProvider(string logfilename)
            {
                _logfilename = logfilename;
            }

            public override void Info(string message)
            {
                _events.Add($"Log information to file '{_logfilename}': {message}");
            }

            public override void Warning(string message)
            {
                _events.Add($"Log warning to file '{_logfilename}': {message}");
            }
        }
    }
}
