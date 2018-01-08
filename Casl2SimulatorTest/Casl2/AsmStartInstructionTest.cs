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
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            const Label DontCare = null;

            CheckReadOperand(
                String.Empty, true, null,
                "空文字列 => OK, 実行開始アドレスは空");
            CheckReadOperand(
                LabelTest.ValidLabelName, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, 実行開始アドレスは指定のラベル");
            CheckReadOperand(
                "OPR1,OPR2", false, DontCare,
                "オペランドが 1 より多い => 例外");
        }

        private void CheckReadOperand(String text, Boolean success, Label expected, String message)
        {
            AsmStartInstruction target = new AsmStartInstruction();
            InstructionTest.CheckReadOperand(target, text, success, message);
            if (success)
            {
                Label actual = target.ExecStartAddress.Label;
                LabelTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            AsmStartInstruction target = new AsmStartInstruction();
            Boolean result = target.IsStart();
            Assert.IsTrue(result, "START 命令の IsStart() => true");
        }
    }
}
