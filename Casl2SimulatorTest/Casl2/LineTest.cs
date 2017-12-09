using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

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
            String str, Boolean success, Label expectedLabel, String expectedMnemonic, String message)
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
                CheckInstruction(actual, expectedMnemonic, "Instruction: " + message);
            }
        }

        private void CheckInstruction(Line actial, String expectedMnemonic, String message)
        {
            if (expectedMnemonic == null)
            {
                Assert.IsNull(actial.Instruction, message);
            }
            else
            {
                Assert.AreEqual(expectedMnemonic, actial.Instruction.Mnemonic, message);
            }
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            Line instructionLine = Line.Parse("LBL001 LD GR1,GR2");
            CheckGenerateCode(
                instructionLine, new Label("LBL001"), WordTest.MakeArray(0x1412),
                "命令行 => ラベルがあれば登録し、その命令のコードを生成する");

            Line commentLine = Line.Parse("; コメント行");
            CheckGenerateCode(
                commentLine, null, WordTest.MakeArray(),
                "コメント行 => ラベル登録も、コード生成も、行わない");

            Line errorLine = Line.Parse(" 未定義命令");
            CheckGenerateCode(
                errorLine, null, WordTest.MakeArray(),
                "エラー行 => ラベル登録も、コード生成も、行わない");
        }

        private void CheckGenerateCode(
            Line target, Label expectedLabel, Word[] expectedWords, String message)
        {
            LabelManager lblManager = new LabelManager();
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(lblManager, relModule);

            CheckLabelIsRegistered(expectedLabel, lblManager, "Label: " + message);
            CheckGeneratedCode(relModule, expectedWords, "Code: " + message);
        }

        private void CheckLabelIsRegistered(Label expectedLabel, LabelManager lblManager, String message)
        {
            if (expectedLabel != null)
            {
                Boolean isRegistered = lblManager.IsRegistered(expectedLabel);
                Assert.IsTrue(isRegistered, message);
            }
        }

        private void CheckGeneratedCode(RelocatableModule relModule, Word[] expectedWords, String message)
        {
            RelocatableModuleTest.Check(relModule, expectedWords, message);
        }
    }
}
