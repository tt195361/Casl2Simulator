using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MacroRpushInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MacroRpushInstructionTest
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
            MacroRpushInstruction target = new MacroRpushInstruction();
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            MacroRpushInstruction target = new MacroRpushInstruction();
            String[] actual = target.ExpandMacro(new Label("LBL001"));

            String[] expected = TestUtils.MakeArray(
                LineTest.MakeGeneratedLine("LBL001", "PUSH", "0,GR1"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR2"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR3"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR4"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR5"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR6"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR7"));
            TestUtils.CheckEnumerable(expected, actual, "マクロ命令 RPUSH の展開結果");
        }
    }
}
