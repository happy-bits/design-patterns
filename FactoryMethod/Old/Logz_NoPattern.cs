
// todo: Lägg till mer logik i LogCreator så det blir mer intressant

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignPatterns.FactoryMethod
{
    [TestClass]
    public class Logz_NoPattern
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

        // Exercise: create the code below (Client and LogCreator)

        class LogCreator
        {
            private readonly string _environment;
            private readonly LogConfig _config;

            public LogCreator(string environment, LogConfig config)
            {
                _environment = environment;
                _config = config;
            }
            internal Logger GetLogger()
            {
                switch (_environment)
                {
                    case "dev":
                        return new FileLogger(_config.FilePath, _config.Rolling);

                    case "prod":

                        return new DatabaseLogger(_config.ConnectionString);

                    default: throw new ArgumentException();
                }
            }
        }

        class Client
        {
            internal static void Run(string environment, LogConfig config)
            {
                Logger logger = new LogCreator(environment, config).GetLogger();
                logger.Trace("A");
                logger.Warning("B");
            }

        }
    }
}
