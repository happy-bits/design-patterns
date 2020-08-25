using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns
{
    public static class TestUtilities
    {
        public static string[] ClassNames(IEnumerable<object> objects) => objects.Select(s => s.GetType().Name).ToArray();

        public static void AssertEqualCollection<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray());
        }
    }
}
