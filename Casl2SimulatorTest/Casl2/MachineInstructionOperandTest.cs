using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// MachineInstructionOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MachineInstructionOperandTest
    {
        #region Fields
        private const MachineInstructionOperand DontCare = null;
        #endregion

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
    }
}
