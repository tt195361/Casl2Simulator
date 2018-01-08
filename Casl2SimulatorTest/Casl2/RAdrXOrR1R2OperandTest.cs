using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// RAdrXOrR1R2Operand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RAdrXOrR1R2OperandTest
    {
        #region Fields
        private const UInt16 RAdrXOpcode = 0x11;
        private const UInt16 R1R2Opcode = 0x22;
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const MachineInstructionOperand DontCare = null;

            CheckParse(
                "GR0,#ABCD,GR1", true,
                RAdrXOperand.MakeForUnitTest(
                    RAdrXOpcode,
                    RegisterOperandTest.GR0,
                    AdrXOperand.MakeForUnitTest(new HexaDecimalConstant(0xABCD), RegisterOperandTest.GR1)),
                "r,adr,x の場合");
            CheckParse(
                "GR0,GR1", true,
                R1R2Operand.MakeForUnitTest(
                    R1R2Opcode, RegisterOperandTest.GR0, RegisterOperandTest.GR1),
                "r1,r2 の場合");

            CheckParse(
                "NoSuchRegister,GR0", false, DontCare,
                "最初のオペランドがレジスタでない場合 => 例外");
            CheckParse(
                "GR0#GR1", false, DontCare,
                "区切りが ',' でない場合 => 例外、'GR#GR1' は項目の区切りまでラベルとして読み込まれる");
            CheckParse(
                "GR0,'abc'", false, DontCare,
                "2 つめのオペランドがレジスタでもアドレスでもない場合 => 例外");
        }

        private void CheckParse(
            String str, Boolean success, MachineInstructionOperand expected, String message)
        {
            MachineInstructionOperand actual = OperandTest.CheckParse(
                (lexer) => RAdrXOrR1R2Operand.Parse(lexer, RAdrXOpcode, R1R2Opcode), str, success, message);
            if (success)
            {
                MachineInstructionOperandTest.Check(expected, actual, Check, message);
            }
        }

        private static void Check(
            MachineInstructionOperand expected, MachineInstructionOperand actual, String message)
        {
            Type expectedType = expected.GetType();
            if (expectedType == typeof(RAdrXOperand))
            {
                RAdrXOperandTest.Check((RAdrXOperand)expected, (RAdrXOperand)actual, message);
            }
            else if (expectedType == typeof(R1R2Operand))
            {
                R1R2OperandTest.Check((R1R2Operand)expected, (R1R2Operand)actual, message);
            }
            else
            {
                Assert.Fail("チェックするオブジェクトが RAdrXOperand でも R1R2Operand でもありません。");
            }
        }
    }
}
