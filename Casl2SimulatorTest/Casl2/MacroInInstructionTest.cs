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
            const String DontCare = null;

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
                "LBL001,LBL002", true, "LBL001", "LBL002",
                "ラベル 2 つ => OK");
            CheckParseOperand(
                "LBL001,LBL002,", false, DontCare, DontCare,
                "ラベル 2 つのあとにまだ文字がある => エラー, 2 つのラベルが必要");
        }

        private void CheckParseOperand(
            String str, Boolean success, String expectedInputBufferArea,
            String expectedInputLengthArea, String message)
        {
            MacroInInstruction target = new MacroInInstruction();
            InstructionTest.CheckParseOperand(target, str, success, message);
            if (success)
            {
                String actualInputBufferArea = target.InputBufferArea;
                Assert.AreEqual(expectedInputBufferArea, actualInputBufferArea, message);

                String actualInputLengthArea = target.InputLengthArea;
                Assert.AreEqual(expectedInputLengthArea, actualInputLengthArea, message);
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
