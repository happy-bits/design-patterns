
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Singleton.Databases.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {
            var x = new Database();
            x.Query("SELECT * ...");

            // some code

            var y = new Database();
            y.Query("SELECT Customer ...");

            CollectionAssert.AreEqual(new[] {
                //"SELECT * ...",
                "SELECT Customer ..."   // "y" innehåller bara den ena SQL-koden eftersom "x" och "y" är olika objekt
            }, y.Commands.ToArray());
        }
    }

    public class Database
    {

        private readonly ConcurrentQueue<string> _commands = new ConcurrentQueue<string>();

        public IEnumerable<string> Commands => _commands;

        public void Query(string sql)
        {
            _commands.Enqueue(sql);
        }
    }

}
