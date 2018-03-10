using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MacroInOutInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MacroInOutInstructionTest
    {
        /// <summary>
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            Label expectedInputBufferArea = new Label("LBL001");
            Label expectedInputLengthArea = new Label("LBL002");
            const Label DontCare = null;

            CheckReadOperand(
                "LBL001,LBL002", true, expectedInputBufferArea, expectedInputLengthArea,
                "ラベル 2 つ => OK");
            CheckReadOperand(
                "LBL001,LBL002,", false, DontCare, DontCare,
                "ラベル 2 つのあとにまだ文字がある => エラー, 2 つのラベルが必要");
        }

        private void CheckReadOperand(
            String text, Boolean success,
            Label expectedAreaSpecBuffer, Label expectedAreaSpecLength, String message)
        {
            MacroInOutInstruction actual = MacroInOutInstruction.MakeIn();
            ProgramInstructionTest.CheckReadOperand(actual, text, success, message);
            if (success)
            {
                LabelTest.Check(expectedAreaSpecBuffer, actual.AreaSpec.Buffer, message);
                LabelTest.Check(expectedAreaSpecLength, actual.AreaSpec.Length, message);
            }
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            MacroInOutInstruction inInstruction = MacroInOutInstruction.MakeIn();
            CheckExpandMacro(inInstruction, 1, "IN 命令の展開結果 => SVC 命令のオペランドは 1");

            MacroInOutInstruction outInstruction = MacroInOutInstruction.MakeOut();
            CheckExpandMacro(outInstruction, 2, "OUT 命令の展開結果 => SVC 命令のオペランドは 2");
        }

        private void CheckExpandMacro(MacroInOutInstruction target, Int32 svcOperand, String message)
        {
            const String BufferName = "BUF";
            const String LengthName = "LEN";
            const String LabelName = "LBL001";

            target.SetLabelsForUnitTest(BufferName, LengthName);
            Label label = new Label(LabelName);

            String[] actual = target.ExpandMacro(label);

            String[] expected = TestUtils.MakeArray(
                LineTest.MakeGeneratedLine(LabelName, "PUSH", "0,GR1"),
                LineTest.MakeGeneratedLine("", "PUSH", "0,GR2"),
                LineTest.MakeGeneratedLine("", "LAD", "GR1," + BufferName),
                LineTest.MakeGeneratedLine("", "LAD", "GR2," + LengthName),
                LineTest.MakeGeneratedLine("", "SVC", svcOperand.ToString()),
                LineTest.MakeGeneratedLine("", "POP", "GR2"),
                LineTest.MakeGeneratedLine("", "POP", "GR1"));
            TestUtils.CheckEnumerable(expected, actual, message);
        }
    }
}
