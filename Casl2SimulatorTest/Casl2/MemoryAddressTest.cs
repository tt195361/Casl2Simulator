using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MemoryAddress"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MemoryAddressTest
    {
        #region Static Fields
        private const UInt16 DontCare = 0;
        #endregion

        /// <summary>
        /// <see cref="MemoryAddress.Add(MemoryOffset)"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Add_Offset()
        {
            CheckAdd_Offset(0xfffe, 1, true, 0xffff, "オフセットを足した結果が最大アドレス以内 => OK");
            CheckAdd_Offset(0xffff, 1, false, DontCare, "オフセットを足した結果が最大アドレスを超える => 例外");
        }

        private void CheckAdd_Offset(
            UInt16 addressValue, UInt16 offsetValue, Boolean success, UInt16 expectedValue, String message)
        {
            CheckAdd(
                addressValue, (address) => AddOffset(address, offsetValue),
                success, expectedValue, message);
        }

        private MemoryAddress AddOffset(MemoryAddress address, UInt16 offsetValue)
        {
            MemoryOffset offset = new MemoryOffset(offsetValue);
            return address.Add(offset);
        }

        /// <summary>
        /// <see cref="MemoryAddress.Add(MemorySize)"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Add_Size()
        {
            CheckAdd_Size(0xfffe, 1, true, 0xffff, "サイズを足した結果が最大アドレス以内 => OK");
            CheckAdd_Size(0xffff, 1, false, DontCare, "サイズを足した結果が最大アドレスを超える => 例外");
        }

        private void CheckAdd_Size(
            UInt16 addressValue, UInt16 sizeValue, Boolean success, UInt16 expectedValue, String message)
        {
            CheckAdd(
               addressValue, (address) => AddSize(address, sizeValue),
               success, expectedValue, message);
        }

        private MemoryAddress AddSize(MemoryAddress address, UInt16 sizeValue)
        {
            MemorySize size = new MemorySize(sizeValue);
            return address.Add(size);
        }

        private void CheckAdd(
            UInt16 addressValue, Func<MemoryAddress, MemoryAddress> testFunc,
            Boolean success, UInt16 expectedValue, String message)
        {
            MemoryAddress target = new MemoryAddress(addressValue);
            try
            {
                MemoryAddress actual = testFunc(target);
                Assert.IsTrue(success, message);

                MemoryAddress expected = new MemoryAddress(expectedValue);
                Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(MemoryAddress expected, MemoryAddress actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
