using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MachineInstructionOperand"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MachineInstructionOperandTest
    {
        #region Instance Fields
        private MachineInstructionOperand m_GR1_GR2;
        private MachineInstructionOperand m_GR3_1111_GR4;
        private MachineInstructionOperand m_GR3_Eq1234_GR4;
        private MachineInstructionOperand m_GR5_2222;
        private MachineInstructionOperand m_3333_GR6;
        private MachineInstructionOperand m_EqSTR_GR6;
        private MachineInstructionOperand m_4444;
        private MachineInstructionOperand m_GR7;
        private MachineInstructionOperand m_NoOperand;

        private const MachineInstructionOperand DontCare = null;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_GR1_GR2 = R1R2Operand.MakeForUnitTest(RegisterOperandTest.GR1, RegisterOperandTest.GR2);
            m_GR3_1111_GR4 = RAdrXOperand.MakeForUnitTest(
                RegisterOperandTest.GR3,
                AdrXOperand.MakeForUnitTest(new DecimalConstant(1111), RegisterOperandTest.GR4));
            m_GR3_Eq1234_GR4 = RAdrXOperand.MakeForUnitTest(
                RegisterOperandTest.GR3,
                AdrXOperand.MakeForUnitTest(
                    Literal.MakeForUnitTest(new DecimalConstant(1234)), RegisterOperandTest.GR4));
            m_GR5_2222 = RAdrXOperand.MakeForUnitTest(
                RegisterOperandTest.GR5,
                AdrXOperand.MakeForUnitTest(new DecimalConstant(2222), null));
            m_3333_GR6 = AdrXOperand.MakeForUnitTest(new DecimalConstant(3333), RegisterOperandTest.GR6);
            m_EqSTR_GR6 = AdrXOperand.MakeForUnitTest(
                Literal.MakeForUnitTest(new StringConstant("STR")), RegisterOperandTest.GR6);
            m_4444 = AdrXOperand.MakeForUnitTest(new DecimalConstant(4444), null);
            m_GR7 = RegisterOperandTest.GR7;
            m_NoOperand = NoOperand.MakeForUnitTest();
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            CheckGenerateLiteralDc(
                m_GR1_GR2, null,
                "r1,r2 => DC 命令は生成されない");
            CheckGenerateLiteralDc(
                m_GR3_1111_GR4, null,
                "r,adr[,x] で adr がリテラルでない => DC 命令は生成されない");
            CheckGenerateLiteralDc(
                m_GR3_Eq1234_GR4,
                ProgramLineTest.MakeGeneratedLine("LTRL0001", "DC", "1234"),
                "r,adr[,x] で adr がリテラル => DC 命令が生成される");
            CheckGenerateLiteralDc(
                m_3333_GR6, null,
                "adr[,x] で adr がリテラルでない => DC 命令は生成されない");
            CheckGenerateLiteralDc(
                m_EqSTR_GR6,
                ProgramLineTest.MakeGeneratedLine("LTRL0001", "DC", "'STR'"),
                "adr[,x] で adr がリテラル => DC 命令が生成される");
        }

        private void CheckGenerateLiteralDc(
            MachineInstructionOperand target, String expected, String message)
        {
            ICodeGeneratorTest.CheckGenerateLiteralDc(target, expected, message);
        }

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            CheckGetCodeWordCount(m_GR1_GR2, 0, "r1,r2 => 0: 追加なし");
            CheckGetCodeWordCount(m_GR3_1111_GR4, 1, "r,adr[,x] => 1: adr で追加 1 ワード");
            CheckGetCodeWordCount(m_3333_GR6, 1, "adr[,x] => 1: adr で追加 1 ワード");
            CheckGetCodeWordCount(m_GR7, 0, "r => 0: 追加なし");
            CheckGetCodeWordCount(m_NoOperand, 0, "オペランドなし => 0: 追加なし");
        }

        private void CheckGetCodeWordCount(MachineInstructionOperand target, Int32 expected, String message)
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(target, expected, message);
        }

        /// <summary>
        /// GetRR1 メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetRR1()
        {
            CheckGetRR1(m_GR1_GR2, 1, "r1,r2: r/r1 は GR1 => 1");
            CheckGetRR1(m_GR3_1111_GR4, 3, "r,adr[,x]: r/r1 は GR3 => 3");
            CheckGetRR1(m_3333_GR6, 0, "adr[,x]: r/r1 なし => 0");
            CheckGetRR1(m_GR7, 7, "r: r/r1 は GR7 => 7");
            CheckGetRR1(m_NoOperand, 0, "なし: r/r1 なし => 0");
        }

        private void CheckGetRR1(
            MachineInstructionOperand target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetRR1();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// GetXR2 メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetXR2()
        {
            CheckGetXR2(m_GR1_GR2, 2, "r1,r2: x/r2 は GR2 => 2");
            CheckGetXR2(m_GR3_1111_GR4, 4, "r,adr,x: x/r2 は GR4 => 4");
            CheckGetXR2(m_GR5_2222, 0, "r,adr: x/r2 は 0 => 0");
            CheckGetXR2(m_3333_GR6, 6, "adr,x: x/r2 は GR6 => 6");
            CheckGetXR2(m_4444, 0, "adr: x/r2 は 0 => 0");
            CheckGetXR2(m_GR7, 0, "r: x/r2 なし => 0");
            CheckGetXR2(m_NoOperand, 0, "なし: x/r2 なし => 0");
        }

        private void CheckGetXR2(
            MachineInstructionOperand target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetXR2();
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check<T>(T expected, T actual, Action<T, T, String> checkAction, String message)
            where T : MachineInstructionOperand
        {
            Assert.AreEqual(expected.Opcode, actual.Opcode, "Opcode: " + message);
            checkAction(expected, actual, message);
        }
    }
}
