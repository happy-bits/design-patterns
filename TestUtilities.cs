using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesignPatterns
{
    public class TestUtilities
    {
        List<string> _log = new List<string>();

        internal void AssertAndClearLog(string[] messages)
        {
            CollectionAssert.AreEqual(messages, _log);
            ClearLog();
        }

        private void ClearLog()
        {
            _log = new List<string>();
        }

        internal void AssertAndClearLog(string message)
        {
            AssertAndClearLog(new[] { message });
        }

        internal void Log(string message)
        {
            _log.Add(message);
        }
    }
}
