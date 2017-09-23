using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tt195361.Casl2SimulatorTest
{
    /// <summary>
    /// 単体テストで共通に使うメソッドです。
    /// </summary>
    internal static class TestUtils
    {
        internal static void CheckArray<T>(T[] expectedArray, T[] actualArray, String message)
        {
            Assert.AreEqual(expectedArray.Length, actualArray.Length, "Length: " + message);

            for (Int32 index = 0; index < expectedArray.Length; ++index)
            {
                T expectedItem = expectedArray[index];
                T actualItem = actualArray[index];
                Assert.AreEqual(expectedItem, actualItem, index.ToString() + ": " + message);
            }
        }

        internal static void CheckTypes(Object[] actualArray, Type[] expectedTypes, String message)
        {
            Assert.AreEqual(expectedTypes.Length, actualArray.Length, "Length: " + message);

            for (Int32 index = 0; index < expectedTypes.Length; ++index)
            {
                Type expectedType = expectedTypes[index];
                Object actualItem = actualArray[index];
                Assert.IsInstanceOfType(actualItem, expectedType, index.ToString() + ": " + message);
            }
        }

        internal static T[] MakeArray<T>(params T[] args)
        {
            return args;
        }
    }
}
