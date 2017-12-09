using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// RAdrXOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RAdrXOperandTest
    {
        #region Fields
        private const UInt16 Opcode = 0x8A;
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const RAdrXOperand DontCare = null;

            CheckParse(
                "GR2,='abc',GR3", true,
                RAdrXOperand.MakeForUnitTest(
                    Opcode,
                    RegisterOperandTest.GR2,
                    AdrXOperand.MakeForUnitTest(
                        Literal.MakeForUnitTest(new StringConstant("abc")),
                        RegisterOperandTest.GR3)),
                "r,adr,x の場合");

            CheckParse(
                "1234,5678", false, DontCare,
                "最初のオペランドがレジスタでない場合 => 例外");
            CheckParse(
                "GR0=#ABCD", false, DontCare,
                "区切りが ',' でない場合 => 例外、'GR0=#ABCD' は項目の区切りまでラベルとして読み込まれる");
            CheckParse(
                "GR0,GR1", false, DontCare,
                "2 つめのオペランドがアドレスでない場合 => 例外");
        }

        private void CheckParse(
            String str, Boolean success, RAdrXOperand expected, String message)
        {
            RAdrXOperand actual = MachineInstructionOperandTest.CheckParse(
                (lexer) => RAdrXOperand.Parse(lexer, Opcode), str, success, message);
            if (success)
            {
                MachineInstructionOperandTest.Check(expected, actual, Check, message);
            }
        }

        /// <summary>
        /// MakeSecondWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void MakeSecondWord()
        {
            const UInt16 Adr = 0xABCD;
            RegisterOperand DontCareRegister = RegisterOperandTest.GR1;
            RAdrXOperand target = RAdrXOperand.MakeForUnitTest(
                DontCareRegister, 
                AdrXOperand.MakeForUnitTest(new HexaDecimalConstant(Adr), DontCareRegister));

            MachineInstructionOperandTest.CheckMakeSecondWord(
                target, new Word(Adr), "第 2 語は adr の値になる");
        }

        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            RAdrXOperand target = RAdrXOperand.MakeForUnitTest(
                RegisterOperandTest.GR4,
                AdrXOperand.MakeForUnitTest(
                    Literal.MakeForUnitTest(new StringConstant("abc")), RegisterOperandTest.GR6));
            String actual = target.ToString();
            Assert.AreEqual("GR4,='abc',GR6", actual, "GR4,='abc',GR6");
        }

        internal static void Check(RAdrXOperand expected, RAdrXOperand actual, String message)
        {
            RegisterOperandTest.Check(expected.R, actual.R, "R: " + message);
            AdrXOperandTest.Check(expected.AdrX, actual.AdrX, "AdrX: " + message);
        }
    }
}
