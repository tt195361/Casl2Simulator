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

        const Boolean DontCareBool = false;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
            m_gr = m_registerSet.GR[1];
            m_fr = m_registerSet.FR;
        }

        #region Load/Store
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
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
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
        #endregion // Arithmetic/Logical Operation

        #region Comparison
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
            CheckSignFlag(target, 0x0001, 0x0001, false, "結果が 0 (1 == 1) => SF は false");
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
            CheckSignFlag(target, 0x0001, 0x0001, false, "結果が 0 (1 == 1) => SF は false");
            CheckSignFlag(target, 0x0001, 0xffff, true, "結果が負の値 (1 < 65535) => SF は true");

            CheckZeroFlag(target, 0x1111, 0x2222, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x1111, 0x1111, true, "結果が 0 => ZF は true");
        }
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// ShiftLeftArithmetic の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmetic()
        {
            Operator target = Operator.ShiftLeftArithmetic;

            CheckRegisterResult(target, 0xaaaa, 1, 0xd554, "左に 1 回算術シフトされる");

            CheckOverflowFlag(target, 0xfffe, 15, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0001, 15, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0x7fff, 15, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x7fff, 15, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// ShiftRightArithmetic の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmetic()
        {
            Operator target = Operator.ShiftRightArithmetic;

            CheckRegisterResult(target, 0xaaaa, 1, 0xd555, "右に 1 回算術シフトされる");

            CheckOverflowFlag(target, 0xfff7, 4, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0008, 4, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0x7fff, 15, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0x7fff, 15, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// ShiftLeftLogical の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogical()
        {
            Operator target = Operator.ShiftLeftLogical;

            CheckRegisterResult(target, 0xaaaa, 1, 0x5554, "左に 1 回論理シフトされる");

            CheckOverflowFlag(target, 0xfffe, 16, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x0001, 16, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0xffff, 16, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x0001, 15, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0xffff, 16, true, "結果が 0 => ZF は true");
        }

        /// <summary>
        /// ShiftRightLogical の単体テストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogical()
        {
            Operator target = Operator.ShiftRightLogical;

            CheckRegisterResult(target, 0xaaaa, 1, 0x5555, "右に 1 回論理シフトされる");

            CheckOverflowFlag(target, 0x7fff, 16, false, "OF は最後に送り出されたビットの値: 0");
            CheckOverflowFlag(target, 0x8000, 16, true, "OF は最後に送り出されたビットの値: 1");

            CheckSignFlag(target, 0xffff, 16, false, "結果が正の値か 0 => SF は false");
            CheckSignFlag(target, 0x8000, 0, true, "結果が負の値 => SF は true");

            CheckZeroFlag(target, 0xffff, 15, false, "結果が 0 以外 => ZF は false");
            CheckZeroFlag(target, 0xffff, 16, true, "結果が 0 => ZF は true");
        }
        #endregion // Shift

        #region Jump
        /// <summary>
        /// JumpOnMinus の単体テストです。
        /// </summary>
        [TestMethod]
        public void JumpOnMinus()
        {
            Operator target = Operator.JumpOnMinus;

            CheckJump(target, DontCareBool, true, DontCareBool, true, "SF=1 => 分岐する");
            CheckJump(target, DontCareBool, false, DontCareBool, false, "SF=0 => 分岐しない");
        }

        /// <summary>
        /// JumpOnNonZero の単体テストです。
        /// </summary>
        [TestMethod]
        public void JumpOnNonZero()
        {
            Operator target = Operator.JumpOnNonZero;

            CheckJump(target, DontCareBool, DontCareBool, false, true, "ZF=0 => 分岐する");
            CheckJump(target, DontCareBool, DontCareBool, true, false, "ZF=1 => 分岐しない");
        }

        /// <summary>
        /// JumpOnZero の単体テストです。
        /// </summary>
        [TestMethod]
        public void JumpOnZero()
        {
            Operator target = Operator.JumpOnZero;

            CheckJump(target, DontCareBool, DontCareBool, true, true, "ZF=1 => 分岐する");
            CheckJump(target, DontCareBool, DontCareBool, false, false, "ZF=0 => 分岐しない");
        }

        /// <summary>
        /// UnconditionalJump の単体テストです。
        /// </summary>
        [TestMethod]
        public void UnconditionalJump()
        {
            Operator target = Operator.UnconditionalJump;

            CheckJump(target, DontCareBool, DontCareBool, DontCareBool, true, "無条件で分岐する");
        }

        /// <summary>
        /// JumpOnPlus の単体テストです。
        /// </summary>
        [TestMethod]
        public void JumpOnPlus()
        {
            Operator target = Operator.JumpOnPlus;

            CheckJump(target, DontCareBool, false, false, true, "SF=0 かつ ZF=0 => 分岐する");
            CheckJump(target, DontCareBool, true, false, false, "SF=1 => 分岐しない");
            CheckJump(target, DontCareBool, false, true, false, "ZF=1 => 分岐しない");
        }

        /// <summary>
        /// JumpOnOverflow の単体テストです。
        /// </summary>
        [TestMethod]
        public void JumpOnOverflow()
        {
            Operator target = Operator.JumpOnOverflow;

            CheckJump(target, true, DontCareBool, DontCareBool, true, "OF=1 => 分岐する");
            CheckJump(target, false, DontCareBool, DontCareBool, false, "OF=0 => 分岐しない");
        }
        #endregion // Jump

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

        private void CheckJump(
            Operator op, Boolean overflowFlag, Boolean signFlag, Boolean zeroFlag, Boolean jump, String message)
        {
            const UInt16 PRValue = 0x1111;
            const UInt16 OperandValue = 0x2222;
            const UInt16 DontCare = 0;

            m_registerSet.FR.SetFlags(overflowFlag, signFlag, zeroFlag);
            m_registerSet.PR.SetValue(PRValue);
            Operate(op, DontCare, OperandValue);

            UInt16 expected = jump ? OperandValue : PRValue;
            UInt16 actual = m_registerSet.PR.Value.GetAsUnsigned();
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
