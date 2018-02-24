using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// MacroInInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MacroInInstructionTest
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
            String text, Boolean success, Label expectedInputBufferArea,
            Label expectedInputLengthArea, String message)
        {
            MacroInInstruction actual = new MacroInInstruction();
            ProgramInstructionTest.CheckReadOperand(actual, text, success, message);
            if (success)
            {
                LabelTest.Check(expectedInputBufferArea, actual.InputArea.Buffer, message);
                LabelTest.Check(expectedInputLengthArea, actual.InputArea.Length, message);
            }
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            MacroInInstruction target = new MacroInInstruction();
            target.SetLabelsForUnitTest("IBUF", "LEN");

            String[] actual = target.ExpandMacro(new Label("LBL001"));

            String[] expected = new String[]
            {
                "LBL001\t" + "PUSH\t" + "0,GR1",
                "\t" +       "PUSH\t" + "0,GR2",
                "\t" +       "LAD\t" +  "GR1,IBUF",
                "\t" +       "LAD\t" +  "GR2,LEN",
                "\t" +       "SVC\t" +  "1",
                "\t" +       "POP\t" +  "GR2",
                "\t" +       "POP\t" +  "GR1",
            };
            TestUtils.CheckEnumerable(expected, actual, "マクロ命令 IN の展開結果");
        }
    }
}
