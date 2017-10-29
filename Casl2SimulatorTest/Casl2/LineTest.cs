using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Line クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LineTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            Label expectedLabel = new Label("LABEL");
            const Label DontCareLabel = null;
            const String DontCareInstructionCode = null;

            CheckParse(
                "; コメント",
                true, null, null, "空白なしで ';' => コメント行");
            CheckParse(
                " ; コメント",
                true, null, null, "空白に続いて ';' => コメント行");

            CheckParse(
                "LABEL DC 123",
                true, expectedLabel, "DC", "ラベルがある場合");
            CheckParse(
                "      DC 123",
                true, null, "DC", "ラベルがない場合");

            CheckParse(
                "LABEL START",
                true, expectedLabel, "START", "オペランドなしで、そのまま終了");
            CheckParse(
                "LABEL START ",
                true, expectedLabel, "START", "オペランドなしで、空白があって終了");
            CheckParse(
                "LABEL START ;コメント",
                true, expectedLabel, "START", "オペランドなしで、';' でコメント開始");
            CheckParse(
                "LABEL START EXECSTRT",
                true, expectedLabel, "START", "オペランドありで、そのまま終了");
            CheckParse(
                "LABEL START EXECSTRT コメント",
                true, expectedLabel, "START", "オペランドありで、コメントあり");

            CheckParse(
                "LABEL DC ' ' コメント",
                true, expectedLabel, "DC", "文字定数中に空白がある場合");
            CheckParse(
                "LABEL DC 10,',',#ABCD コメント",
                true, expectedLabel, "DC", "文字定数中にコンマがある場合");

            CheckParse(
                String.Empty,
                false, DontCareLabel, DontCareInstructionCode, "空行 => エラー");
        }

        private void CheckParse(
            String str, Boolean success, Label expectedLabel, String expectedInstructionCode, String message)
        {
            Line actual = Line.Parse(str);

            if (!success)
            {
                Assert.IsNotNull(actual.ErrorMessage, "ErrorMessage: " + message);
                Assert.IsNull(actual.Label, "Label: " + message);
                Assert.IsNull(actual.Instruction, "Instruction: " + message);
            }
            else
            {
                Assert.IsNull(actual.ErrorMessage, "ErrorMessage: " + message);
                LabelTest.Check(expectedLabel, actual.Label, "Label: " + message);
                CheckInstructionType(actual, expectedInstructionCode, "Instruction: " + message);
            }
        }

        private void CheckInstructionType(Line actial, String expectedInstructionCode, String message)
        {
            if (expectedInstructionCode == null)
            {
                Assert.IsNull(actial.Instruction, message);
            }
            else
            {
                Assert.AreEqual(expectedInstructionCode, actial.Instruction.Code, message);
            }
        }
    }
}
