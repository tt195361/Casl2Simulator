using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MacroRpopInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MacroRpopInstructionTest
    {
        /// <summary>
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            CheckReadOperand(
                String.Empty, true, "オペランドなし => OK");
            CheckReadOperand(
                "OPR", false, "オペランドがある => 例外");
        }

        private void CheckReadOperand(String text, Boolean success, String message)
        {
            MacroRpopInstruction target = new MacroRpopInstruction();
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            MacroRpopInstruction target = new MacroRpopInstruction();
            String[] actual = target.ExpandMacro(new Label("LBL001"));

            String[] expected = TestUtils.MakeArray(
                LineTest.MakeGeneratedLine("LBL001", "POP", "GR7"),
                LineTest.MakeGeneratedLine("", "POP", "GR6"),
                LineTest.MakeGeneratedLine("", "POP", "GR5"),
                LineTest.MakeGeneratedLine("", "POP", "GR4"),
                LineTest.MakeGeneratedLine("", "POP", "GR3"),
                LineTest.MakeGeneratedLine("", "POP", "GR2"),
                LineTest.MakeGeneratedLine("", "POP", "GR1"));
            TestUtils.CheckEnumerable(expected, actual, "マクロ命令 RPOP の展開結果");
        }
    }
}
