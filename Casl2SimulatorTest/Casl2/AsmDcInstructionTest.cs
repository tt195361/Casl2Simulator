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
        #region Fields
        private static readonly Type NumericConstant = typeof(NumericConstant);
        private static readonly Type StringConstant = typeof(StringConstant);
        private static readonly Type AddressConstant = typeof(AddressConstant);
        #endregion

        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            CheckParseOperand(
                "'文字定数',12345,L001,#ABCD",
                TestUtils.MakeArray(StringConstant, NumericConstant, AddressConstant, NumericConstant),
                "定数の並び");
            CheckParseOperand(
                String.Empty, null,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseOperand(
                "; コメント", null,
                "コメントでオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseOperand(
                "'abc'123", null,
                "区切りのコンマがない、定数の後ろに解釈できない文字列がある => エラー");
        }

        private void CheckParseOperand(String str, Type[] expectedTypes, String message)
        {
            AsmDcInstruction target = new AsmDcInstruction();
            Boolean success = (expectedTypes != null);
            InstructionTest.CheckParseOperand(target, str, success, message);
            if (success)
            {
                Constant[] actualConstants = target.Constants;
                TestUtils.CheckTypes(actualConstants, expectedTypes, message);
            }
        }
    }
}
