using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest
{
    /// <summary>
    /// 単体テストで共通に使うメソッドです。
    /// </summary>
    internal static class TestUtils
    {
        internal static void CheckArray<T>(T[] expectedArray, T[] actualArray, String message)
        {
            CheckArray<T>(expectedArray, actualArray, Assert.AreEqual, message);
        }

        internal static void CheckArray<T>(
            T[] expectedArray, T[] actualArray, Action<T, T, String> checkAtion, String message)
        {
            Assert.AreEqual(expectedArray.Length, actualArray.Length, "Length: " + message);

            for (Int32 index = 0; index < expectedArray.Length; ++index)
            {
                T expectedItem = expectedArray[index];
                T actualItem = actualArray[index];
                checkAtion(expectedItem, actualItem, index.ToString() + ": " + message);
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
