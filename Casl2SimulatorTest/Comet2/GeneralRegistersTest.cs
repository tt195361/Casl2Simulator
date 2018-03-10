using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="GeneralRegisters"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class GeneralRegistersTest
    {
        #region Instance Fields
        private GeneralRegisters m_gr;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_gr = new GeneralRegisters();
        }

        /// <summary>
        /// インデクサの引数 grNumber の値の範囲のテストです。
        /// </summary>
        [TestMethod]
        public void IndexerGrNumberRange()
        {
            CheckIndexerGrNumberRange(-1, false, "-1: 最小より小さい => 失敗");
            CheckIndexerGrNumberRange(0, true, "0: ちょうど最小 => 成功");
            CheckIndexerGrNumberRange(7, true, "7: ちょうど最大 => 成功");
            CheckIndexerGrNumberRange(8, false, "8: 最大より大きい => 失敗");
        }

        private void CheckIndexerGrNumberRange(Int32 grNumber, Boolean success, String message)
        {
            try
            {
                Register notUsed = m_gr[grNumber];
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
