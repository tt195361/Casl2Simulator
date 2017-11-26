using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// MachineInstructionOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MachineInstructionOperandTest
    {
        #region Fields
        private MachineInstructionOperand m_GR1_GR2;
        private MachineInstructionOperand m_GR3_1111_GR4;
        private MachineInstructionOperand m_GR5_2222;
        private MachineInstructionOperand m_3333_GR6;
        private MachineInstructionOperand m_4444;
        private MachineInstructionOperand m_GR7;
        private MachineInstructionOperand m_NoOperand;

        private const MachineInstructionOperand DontCare = null;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_GR1_GR2 = new R1R2Operand(RegisterOperandTest.GR1, RegisterOperandTest.GR2);
            m_GR3_1111_GR4 = new RAdrXOperand(
                RegisterOperandTest.GR3,
                AdrXOperand.MakeForUnitTest(new DecimalConstant(1111), RegisterOperandTest.GR4));
            m_GR5_2222 = new RAdrXOperand(
                RegisterOperandTest.GR5,
                AdrXOperand.MakeForUnitTest(new DecimalConstant(2222), null));
            m_3333_GR6 = AdrXOperand.MakeForUnitTest(new DecimalConstant(3333), RegisterOperandTest.GR6);
            m_4444 = AdrXOperand.MakeForUnitTest(new DecimalConstant(4444), null);
            m_GR7 = RegisterOperandTest.GR7;
            m_NoOperand = NoOperand.Instance;
        }

        /// <summary>
        /// ParseR1R2OrRAdrX メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseR1R2OrRAdrX()
        {
            CheckParseR1R2OrRAdrX(
                "GR0,GR1", true,
                new R1R2Operand(RegisterOperandTest.GR0, RegisterOperandTest.GR1),
                "r1,r2 の場合");
            CheckParseR1R2OrRAdrX(
                "GR0,#ABCD,GR1", true,
                new RAdrXOperand(
                    RegisterOperandTest.GR0,
                    AdrXOperand.MakeForUnitTest(new HexaDecimalConstant(0xABCD), RegisterOperandTest.GR1)),
                "r,adr,x の場合");

            CheckParseR1R2OrRAdrX(
                "NoSuchRegister,GR0", false, DontCare,
                "最初のオペランドがレジスタでない場合 => 例外");
            CheckParseR1R2OrRAdrX(
                "GR0#GR1", false, DontCare,
                "区切りが ',' でない場合 => 例外、'GR#GR1' は項目の区切りまでラベルとして読み込まれる");
            CheckParseR1R2OrRAdrX(
                "GR0,'abc'", false, DontCare,
                "2 つめのオペランドがレジスタでもアドレスでもない場合 => 例外");
        }

        private void CheckParseR1R2OrRAdrX(
            String str, Boolean success, MachineInstructionOperand expected, String message)
        {
            MachineInstructionOperand actual = CheckParse(
                MachineInstructionOperand.ParseR1R2OrRAdrX, str, success, message);
            if (success)
            {
                Check(expected, actual, message);
            }
        }

        /// <summary>
        /// ParseRAdrX メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseRAdrX()
        {
            CheckParseRAdrX(
                "GR2,='abc',GR3", true,
                new RAdrXOperand(
                    RegisterOperandTest.GR2,
                    AdrXOperand.MakeForUnitTest(
                        Literal.MakeForUnitTest(new StringConstant("abc")),
                        RegisterOperandTest.GR3)),
                "r,adr,x の場合");

            CheckParseRAdrX(
                "1234,5678", false, DontCare,
                "最初のオペランドがレジスタでない場合 => 例外");
            CheckParseRAdrX(
                "GR0=#ABCD", false, DontCare,
                "区切りが ',' でない場合 => 例外、'GR0=#ABCD' は項目の区切りまでラベルとして読み込まれる");
            CheckParseRAdrX(
                "GR0,GR1", false, DontCare,
                "2 つめのオペランドがアドレスでない場合 => 例外");
        }

        private void CheckParseRAdrX(
            String str, Boolean success, MachineInstructionOperand expected, String message)
        {
            MachineInstructionOperand actual = CheckParse(
                MachineInstructionOperand.ParseRAdrX, str, success, message);
            if (success)
            {
                Check(expected, actual, message);
            }
        }

        internal static T CheckParse<T>(
            Func<OperandLexer, T> parseFunc, String str, Boolean success, String message)
        {
            OperandLexer lexer = OperandLexerTest.MakeFrom(str);
            lexer.MoveNext();
            try
            {
                T result = parseFunc(lexer);
                Assert.IsTrue(success, message);
                return result;
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
                return default(T);
            }
        }

        /// <summary>
        /// GetAdditionalWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetAdditionalWordCount()
        {
            CheckGetAdditionalWordCount(m_GR1_GR2, 0, "r1,r2 => 0: 追加なし");
            CheckGetAdditionalWordCount(m_GR3_1111_GR4, 1, "r,adr[,x] => 1: adr で追加 1 ワード");
            CheckGetAdditionalWordCount(m_3333_GR6, 1, "adr[,x] => 1: adr で追加 1 ワード");
            CheckGetAdditionalWordCount(m_GR7, 0, "r => 0: 追加なし");
            CheckGetAdditionalWordCount(m_NoOperand, 0, "オペランドなし => 0: 追加なし");
        }

        private void CheckGetAdditionalWordCount(
            MachineInstructionOperand target, Int32 expected, String message)
        {
            Int32 actual = target.GetAdditionalWordCount();
            Assert.AreEqual(expected, actual, message);
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

        private void Check(MachineInstructionOperand expected, MachineInstructionOperand actual, String message)
        {
            TestUtils.CheckType(expected, actual, message);

            Type expectedType = expected.GetType();
            if (expectedType == typeof(R1R2Operand))
            {
                R1R2OperandTest.Check((R1R2Operand)expected, (R1R2Operand)actual, message);
            }
            else if (expectedType == typeof(RAdrXOperand))
            {
                RAdrXOperandTest.Check((RAdrXOperand)expected, (RAdrXOperand)actual, message);
            }
            else
            {
                Assert.Fail("未知の MachineInstructionOperand の派生クラスです。");
            }
        }

        internal static void CheckMakeSecondWord(
            MachineInstructionOperand target, Word? expected, String message)
        {
            LabelManager lblManager = new LabelManager();
            Word? actual = target.MakeSecondWord(lblManager);
            WordTest.Check(expected, actual, message);
        }
    }
}
