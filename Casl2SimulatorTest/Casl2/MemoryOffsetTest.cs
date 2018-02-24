using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MemoryOffset"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MemoryOffsetTest
    {
        /// <summary>
        /// Add メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Add()
        {
            const UInt16 DontCare = 0;

            CheckAdd(0x0000, -1, false, DontCare, "足した結果が最小アドレスより小さい => 例外");
            CheckAdd(0x0001, -1, true, 0x0000, "足した結果が最小アドレス以内 => OK");
            CheckAdd(0xfffe, 1, true, 0xffff, "足した結果が最大アドレス以内 => OK");
            CheckAdd(0xffff, 1, false, DontCare, "足した結果が最大アドレスを超える => 例外");
        }

        private void CheckAdd(
            UInt16 value, Int32 addend, Boolean success, UInt16 expectedValue, String message)
        {
            MemoryOffset target = new MemoryOffset(value);
            try
            {
                MemoryOffset actual = target.Add(addend);
                Assert.IsTrue(success, message);

                MemoryOffset expected = new MemoryOffset(expectedValue);
                Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(MemoryOffset expected, MemoryOffset actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
