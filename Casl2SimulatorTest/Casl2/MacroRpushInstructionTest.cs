using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// MacroRpushInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MacroRpushInstructionTest
    {
        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            CheckParseOperand(
                String.Empty, true, "オペランドなし => OK");
            CheckParseOperand(
                "OPR", false, "オペランドがある => 例外");
        }

        private void CheckParseOperand(String str, Boolean success, String message)
        {
            MacroRpushInstruction target = new MacroRpushInstruction();
            InstructionTest.CheckParseOperand(target, str, success, message);
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            MacroRpushInstruction target = new MacroRpushInstruction();
            String[] actual = target.ExpandMacro("LBL001");

            String[] expected = new String[]
            {
                "LBL001\t" + "PUSH\t" + "0,GR1",
                "\t" +       "PUSH\t" + "0,GR2",
                "\t" +       "PUSH\t" + "0,GR3",
                "\t" +       "PUSH\t" + "0,GR4",
                "\t" +       "PUSH\t" + "0,GR5",
                "\t" +       "PUSH\t" + "0,GR6",
                "\t" +       "PUSH\t" + "0,GR7",
            };
            TestUtils.CheckArray(expected, actual, "マクロ命令 RPUSH の展開結果");
        }
    }
}
