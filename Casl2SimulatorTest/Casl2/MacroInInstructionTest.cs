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
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            Label expectedInputBufferArea = new Label("LBL001");
            Label expectedInputLengthArea = new Label("LBL002");
            const Label DontCare = null;

            CheckParseOperand(
                String.Empty, false, DontCare, DontCare,
                "オペランドなし => エラー, 2 つのラベルが必要");
            CheckParseOperand(
                "LBL001", false, DontCare, DontCare,
                "ラベル 1 つ => エラー, 2 つのラベルが必要");
            CheckParseOperand(
                "LBL001,", false, DontCare, DontCare,
                "ラベル 1 つとコンマ => エラー, 2 つのラベルが必要");
            CheckParseOperand(
                "LBL001,LBL002", true, expectedInputBufferArea, expectedInputLengthArea,
                "ラベル 2 つ => OK");
            CheckParseOperand(
                "LBL001,LBL002,", false, DontCare, DontCare,
                "ラベル 2 つのあとにまだ文字がある => エラー, 2 つのラベルが必要");
        }

        private void CheckParseOperand(
            String str, Boolean success, Label expectedInputBufferArea,
            Label expectedInputLengthArea, String message)
        {
            MacroInInstruction actual = new MacroInInstruction();
            InstructionTest.CheckParseOperand(actual, str, success, message);
            if (success)
            {
                LabelTest.Check(expectedInputBufferArea, actual.InputBufferArea, message);
                LabelTest.Check(expectedInputLengthArea, actual.InputLengthArea, message);
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

            String[] actual = target.ExpandMacro("LBL001");

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
            TestUtils.CheckArray(expected, actual, "マクロ命令 IN の展開結果");
        }
    }
}
