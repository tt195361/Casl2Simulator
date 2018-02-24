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
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            const Constant[] DontCare = null;

            CheckReadOperand(
                "'StrConst',12345,L001,#ABCD", true,
                ConstantTest.MakeArray(
                    new StringConstant("StrConst"),
                    new DecimalConstant(12345),
                    new AddressConstant("L001"),
                    new HexaDecimalConstant(0xABCD)),
                "定数の並び");
            CheckReadOperand(
                String.Empty, false, DontCare,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckReadOperand(
                "; コメント", false, DontCare,
                "コメントでオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckReadOperand(
                "'abc'123", false, DontCare,
                "区切りのコンマではなく別の字句要素がある => エラー");
        }

        private void CheckReadOperand(
            String text, Boolean success, Constant[] expectedConstants, String message)
        {
            AsmDcInstruction target = new AsmDcInstruction();
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
            if (success)
            {
                ConstantCollection actualConstants = target.Constants;
                ConstantCollectionTest.Check(expectedConstants, actualConstants, message);
            }
        }
    }
}
