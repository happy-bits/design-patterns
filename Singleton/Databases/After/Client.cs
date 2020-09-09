
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Singleton.Databases.After
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var x = Database.Instance;
            x.Query("SELECT * ...");

            // some code

            var y = Database.Instance;
            y.Query("SELECT Customer ...");

            CollectionAssert.AreEqual(new[] {
                "SELECT * ...",
                "SELECT Customer ..."
            }, y.Commands.ToArray());
        }
    }

    public sealed class Database
    {
        private static readonly Lazy<Database> _lazy = new Lazy<Database>(() => new Database());

        public static Database Instance => _lazy.Value;

        private Database() { }

        private readonly ConcurrentQueue<string> _commands = new ConcurrentQueue<string>();

        public IEnumerable<string> Commands => _commands;

        public void Query(string sql)
        {
            _commands.Enqueue(sql);
        }
    }

    // Enklare lösning, men inte trådsäker

    class Database_NonThreadSafe
    {
        private Database_NonThreadSafe() { }

        private static Database_NonThreadSafe _instance;

        public static Database_NonThreadSafe Instance()
        {
            if (_instance == null)
            {
                _instance = new Database_NonThreadSafe();
            }
            return _instance;
        }

        private readonly Queue<string> _commands = new Queue<string>();

        public IEnumerable<string> Commands => _commands;

        public void Query(string sql)
        {
            _commands.Enqueue(sql);
        }
    }
}
