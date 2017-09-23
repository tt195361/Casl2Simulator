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
            const String DontCare = null;

            CheckParseOperand(
                String.Empty, true, null,
                "空文字列 => OK, 実行開始アドレスは空");
            CheckParseOperand(
                "; コメント", true, null,
                "';' で開始 => OK, 実行開始アドレスは空");
            CheckParseOperand(
                LabelTest.ValidLabel, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, 実行開始アドレスは指定のラベル");
            CheckParseOperand(
                LabelTest.InvalidLabel, false, DontCare,
                "オペランド 1 つ、不正なラベル => 例外");
            CheckParseOperand(
                "OPR1,OPR2", false, DontCare,
                "オペランドが 1 より多い => 例外");
        }

        private void CheckParseOperand(String str, Boolean success, String expected, String message)
        {
            AsmStartInstruction target = new AsmStartInstruction();
            InstructionTest.CheckParseOperand(target, str, success, message);
            if (success)
            {
                String actual = target.ExecStartAddress;
                Assert.AreEqual(expected, actual, message);
            }
        }
    }
}
