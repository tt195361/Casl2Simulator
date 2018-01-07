using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// OffsetCalculator クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OffsetCalculatorTest
    {
        /// <summary>
        /// Add メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Add()
        {
            const UInt16 DontCare = 0;

            CheckAdd(0xfffe, 1, true, 0xffff, "足した結果が最大アドレス以内 => OK");
            CheckAdd(0xffff, 1, false, DontCare, "足した結果が最大アドレスを超える => 例外");
        }

        private void CheckAdd(UInt16 offset, Int32 addend, Boolean success, UInt16 expected, String message)
        {
            try
            {
                UInt16 actual = OffsetCalculator.Add(offset, addend);
                Assert.IsTrue(success, message);
                Assert.AreEqual(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
