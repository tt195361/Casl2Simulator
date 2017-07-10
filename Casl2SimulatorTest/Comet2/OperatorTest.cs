using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Operator クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OperatorTest
    {
        #region Fields
        private RegisterSet m_registerSet;
        private Memory m_memory;
        private Register m_gr;
        private FlagRegister m_fr;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
            m_gr = m_registerSet.GR[1];
            m_fr = m_registerSet.FR;
        }

        /// <summary>
        /// Load の単体テストです。
        /// </summary>
        [TestMethod]
        public void Load()
        {
            Operator target = Operator.Load;

            CheckRegisterResult(target, 0, 13579, 13579, "オペランドの値がレジスタに設定される");

            CheckOverflowFlag(target, 0, 0xffff, false, "OF は常に false");

            CheckSignFlag(target, 0, 0x7fff, false, "正の値をロード => SF は false");
            CheckSignFlag(target, 0, 0x8000, true, "負の値をロード => SF は true");

            CheckZeroFlag(target, 0, 1, false, "0 以外をロード => ZF は false");
            CheckZeroFlag(target, 0, 0, true, "0 をロード => ZF は true");
        }

        /// <summary>
        /// Store の単体テストです。
        /// </summary>
        [TestMethod]
        public void Store()
        {
            Operator target = Operator.Store;

            CheckMemoryResult(target, 1234, 56789, 1234, "レジスタの値がオペランドで指定のアドレスに書き込まれる");
        }

        /// <summary>
        /// AddArithmetic の単体テストです。
        /// </summary>
        [TestMethod]
        public void AddArithmetic()
        {
            Operator target = Operator.AddArithmetic;

            CheckRegisterResult(target, 1111, 2222, 3333, "算術加算の値がレジスタに設定される");

            CheckOverflowFlag(target, 20000, 12767, false, "オーバーフローしない => OF は false");
            CheckOverflowFlag(target, 20000, 12768, true, "オーバーフローする => OF は true");

            CheckSignFlag(target, 0x7000, 0x0fff, false, "結果が正の値 => SF は false");
            CheckSignFlag(target, 0x8001, 0xffff, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 2, 0xffff, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 1, 0xffff, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// CompareArithmetic の単体テストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmetic()
        {
            Operator target = Operator.CompareArithmetic;

            CheckRegisterResult(target, 11111, 22222, 11111, "レジスタの値は変わらない");

            CheckOverflowFlag(target, 0, 0x1234, false, "OF は常に false");

            CheckSignFlag(target, 0x0001, 0xffff, false, "結果が正の値 (1 > -1) => SF は false");
            CheckSignFlag(target, 0xffff, 0x0001, true, "結果が負の値 (-1 < 1) => SF は true");

            CheckZeroFlag(target, 0x1111, 0x2222, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x1111, 0x1111, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// CompareLogical の単体テストです。
        /// </summary>
        [TestMethod]
        public void CompareLogical()
        {
            Operator target = Operator.CompareLogical;

            CheckRegisterResult(target, 33333, 44444, 33333, "レジスタの値は変わらない");

            CheckOverflowFlag(target, 0, 0x1234, false, "OF は常に false");

            CheckSignFlag(target, 0xffff, 0x0001, false, "結果が正の値 (65535 > 1) => SF は false");
            CheckSignFlag(target, 0x0001, 0xffff, true, "結果が負の値 (1 < 65535) => SF は true");

            CheckZeroFlag(target, 0x1111, 0x2222, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x1111, 0x1111, true, "結果が 0 => ZF は true");
        }

        private void CheckRegisterResult(
            Operator op, UInt16 regValue, UInt16 oprValue, UInt16 expected, String message)
        {
            Operate(op, regValue, oprValue);
            UInt16 actual = m_gr.Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckMemoryResult(
            Operator op, UInt16 regValue, UInt16 oprValue, UInt16 expected, String message)
        {
            Operate(op, regValue, oprValue);
            Word word = m_memory.Read(oprValue);
            UInt16 actual = word.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckOverflowFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.OF;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckSignFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.SF;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckZeroFlag(
            Operator op, UInt16 regValue, UInt16 oprValue, Boolean expected, String message)
        {
            Operate(op, regValue, oprValue);
            Boolean actual = m_fr.ZF;
            Assert.AreEqual(expected, actual, message);
        }

        private void Operate(Operator op, UInt16 regValue, UInt16 oprValue)
        {
            m_gr.Value = new Word(regValue);
            Word operand = new Word(oprValue);
            op.Operate(m_gr, operand, m_registerSet, m_memory);
        }
    }
}
