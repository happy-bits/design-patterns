using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns
{
    public static class TestUtilities
    {
        public static string[] ClassNames(IEnumerable<object> objects) => objects.Select(s => s.GetType().Name).ToArray();

        public static void AssertEqualCollection(IEnumerable<double> expected, IEnumerable<double> actual)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());
        }

        //List<string> _log = new List<string>();

        //internal void AssertAndClearLog(string[] messages)
        //{
        //    CollectionAssert.AreEqual(messages, _log);
        //    ClearLog();
        //}

        //private void ClearLog()
        //{
        //    _log = new List<string>();
        //}

        //internal void AssertAndClearLog(string message)
        //{
        //    AssertAndClearLog(new[] { message });
        //}

        //internal void Log(string message)
        //{
        //    _log.Add(message);
        //}
    }
}
