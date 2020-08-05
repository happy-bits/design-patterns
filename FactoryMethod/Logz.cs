
// Denna är sämre än no pattern...
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Logz
    {
        [TestMethod]
        public void Ex1()
        {
            Client.Run("dev", new LogConfig 
            { 
                FilePath="c:\\log.txt",
                Rolling=true
            });

            CollectionAssert.AreEqual(new[] {
                $"Trace:A (FileLogger,Filepath:c:\\log.txt,RollingFiles:True)",
                $"Warning:B (FileLogger,Filepath:c:\\log.txt,RollingFiles:True)",
            }, _events);

            _events.Clear();

            Client.Run("prod", new LogConfig
            {
                ConnectionString="CS"
            });

            CollectionAssert.AreEqual(new[] {
                $"Trace:A (DatabaseLogger,ConnectionString:CS)",
                $"Warning:B (DatabaseLogger,ConnectionString:CS)",
            }, _events);
        }

        static List<string> _events = new List<string>();

        abstract class Logger
        {
            public abstract void Trace(string message);
            public abstract void Warning(string message);
        }

        class FileLogger : Logger
        {
            private string _filepath;
            private bool _rollingFiles;

            public FileLogger(string filepath, bool rollingFiles)
            {
                _filepath = filepath;
                _rollingFiles = rollingFiles;
            }

            public override void Trace(string message)
            {
                _events.Add($"Trace:{message} (FileLogger,Filepath:{_filepath},RollingFiles:{_rollingFiles})");
            }

            public override void Warning(string message)
            {
                _events.Add($"Warning:{message} (FileLogger,Filepath:{_filepath},RollingFiles:{_rollingFiles})");
            }
        }
        class DatabaseLogger : Logger
        {
            private string _connectionString;

            public DatabaseLogger(string connectionString)
            {
                _connectionString = connectionString;
            }

            public override void Trace(string message)
            {
                _events.Add($"Trace:{message} (DatabaseLogger,ConnectionString:{_connectionString})");
            }

            public override void Warning(string message)
            {
                _events.Add($"Warning:{message} (DatabaseLogger,ConnectionString:{_connectionString})");
            }
        }

        class LogConfig
        {
            public string ConnectionString { get; set; }
            public string FilePath { get; set; }
            public bool Rolling { get; set; }
            public bool IsOn { get; set; }
        }

        // "Creator"

        abstract class LogCreator
        {
            // "Factory method"
            public abstract Logger Create(LogConfig config);
        }

        // "Concrete creator"

        class FileLoggerCreator : LogCreator
        {
            public override Logger Create(LogConfig config)
            {
                return new FileLogger(config.FilePath, config.Rolling);
            }
        }

        // "Concrete creator"

        class DatabaseLoggerCreator : LogCreator
        {
            public override Logger Create(LogConfig config)
            {
                return new DatabaseLogger(config.ConnectionString);
            }
        }

        class Client
        {
            internal static void Run(string environment, LogConfig config)
            {
                LogCreator logCreator = GetLoggCreator(environment);
                Logger logger = logCreator.Create(config);
                logger.Trace("A");
                logger.Warning("B");
            }

            private static LogCreator GetLoggCreator(string environment)
            {
                switch (environment)
                {
                    case "dev":
                        return new FileLoggerCreator();

                    case "prod":
                        return new DatabaseLoggerCreator();

                    default: throw new ArgumentException();
                }
            }
        }
    }
}
