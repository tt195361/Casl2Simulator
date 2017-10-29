using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AsmDcInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmDcInstructionTest
    {
        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            const Constant[] DontCare = null;

            CheckParseOperand(
                "'文字定数',12345,L001,#ABCD", true,
                ConstantTest.MakeArray(
                    new StringConstant("文字定数"),
                    new DecimalConstant(12345),
                    new AddressConstant("L001"),
                    new HexaDecimalConstant(0xABCD)),
                "定数の並び");
            CheckParseOperand(
                String.Empty, false, DontCare,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseOperand(
                "; コメント", false, DontCare,
                "コメントでオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseOperand(
                "'abc'123", false, DontCare,
                "区切りのコンマではなく別の字句要素がある => エラー");
        }

        private void CheckParseOperand(
            String str, Boolean success, Constant[] expectedConstants, String message)
        {
            AsmDcInstruction target = new AsmDcInstruction();
            InstructionTest.CheckParseOperand(target, str, success, message);
            if (success)
            {
                Constant[] actualConstants = target.Constants;
                TestUtils.CheckArray(expectedConstants, actualConstants, ConstantTest.Check, message);
            }
        }
    }
}
