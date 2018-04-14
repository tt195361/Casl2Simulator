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
        /// <summary>
        /// <see cref="MemoryAddress.Add"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Add()
        {
            const UInt16 DontCare = 0;

            CheckAdd(0xfffe, 1, true, 0xffff, "足した結果が最大アドレス以内 => OK");
            CheckAdd(0xffff, 1, false, DontCare, "足した結果が最大アドレスを超える => 例外");
        }

        private void CheckAdd(
            UInt16 addressValue, UInt16 offsetValue, Boolean success, UInt16 expectedValue, String message)
        {
            MemoryAddress target = new MemoryAddress(addressValue);
            MemoryOffset addend = new MemoryOffset(offsetValue);
            try
            {
                MemoryAddress actual = target.Add(addend);
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
