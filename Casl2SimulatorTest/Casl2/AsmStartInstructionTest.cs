using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AsmStartInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmStartInstructionTest
    {
        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            const Label DontCare = null;

            CheckParseOperand(
                String.Empty, true, null,
                "空文字列 => OK, 実行開始アドレスは空");
            CheckParseOperand(
                LabelTest.ValidLabelName, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, 実行開始アドレスは指定のラベル");
            CheckParseOperand(
                "OPR1,OPR2", false, DontCare,
                "オペランドが 1 より多い => 例外");
        }

        private void CheckParseOperand(String str, Boolean success, Label expected, String message)
        {
            AsmStartInstruction target = new AsmStartInstruction();
            InstructionTest.CheckParseOperand(target, str, success, message);
            if (success)
            {
                Label actual = target.ExecStartAddress;
                LabelTest.Check(expected, actual, message);
            }
        }
    }
}
