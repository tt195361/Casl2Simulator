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
                ProgramLineTest.MakeGeneratedLine("LBL001", "POP", "GR7"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR6"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR5"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR4"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR3"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR2"),
                ProgramLineTest.MakeGeneratedLine("", "POP", "GR1"));
            TestUtils.CheckEnumerable(expected, actual, "マクロ命令 RPOP の展開結果");
        }
    }
}
