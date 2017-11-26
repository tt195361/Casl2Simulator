using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tt195361.Casl2SimulatorTest
{
    /// <summary>
    /// 単体テストで共通に使うメソッドです。
    /// </summary>
    internal static class TestUtils
    {
        internal static void CheckEnumerable<T>(
            IEnumerable<T> expectedItems, IEnumerable<T> actualItems, String message)
        {
            CheckEnumerable(expectedItems, actualItems, Assert.AreEqual, message);
        }

        internal static void CheckEnumerable<T>(
            IEnumerable<T> expectedItems, IEnumerable<T> actualItems,
            Action<T, T, String> checkAction, String message)
        {
            Int32 expectedCount = expectedItems.Count();
            Int32 actualCount = actualItems.Count();
            Assert.AreEqual(expectedCount, actualCount, "Count: " + message);

            expectedItems.ZipAction(
                actualItems,
                (expectedItem, actualItem) => checkAction(expectedItem, actualItem, message));
        }

        private static void ZipAction<TFirst, TSecond>(
            this IEnumerable<TFirst> firstEnumerable, IEnumerable<TSecond> secondEnumerable,
            Action<TFirst, TSecond> action)
        {
            using (IEnumerator<TFirst> firstEnumerator = firstEnumerable.GetEnumerator())
            {
                using (IEnumerator<TSecond> secondEnumerator = secondEnumerable.GetEnumerator())
                {
                    while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                    {
                        action(firstEnumerator.Current, secondEnumerator.Current);
                    }
                }
            }
        }

        internal static T[] MakeArray<T>(params T[] args)
        {
            if (args == null)
            {
                // MakeArray<T>(null) と書くと、args が null になる。
                // 長さが 1 で、要素が null の配列を作りたい。
                return new T[] { default(T) };
            }
            else
            {
                return args;
            }
        }

        internal static void CheckType(Object expected, Object actual, String message)
        {
            Type expectedType = expected.GetType();
            Type actualType = actual.GetType();
            Assert.AreSame(expectedType, actualType, "Type: " + message);
        }
    }
}
