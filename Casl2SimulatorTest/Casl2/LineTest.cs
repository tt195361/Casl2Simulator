using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Line"/> クラスの単体テストです。
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
            const String NullInstructionMnemonic = "NULL";

            CheckParse(
                "; コメント",
                true, null, NullInstructionMnemonic, "空白なしで ';' => コメント行");
            CheckParse(
                " ; コメント",
                true, null, NullInstructionMnemonic, "空白に続いて ';' => コメント行");

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
                false, DontCareLabel, NullInstructionMnemonic, "空行 => エラー");
        }

        private void CheckParse(
            String text, Boolean success, Label expectedLabel, String expectedMnemonic, String message)
        {
            Line actual = Line.Parse(text);

            if (!success)
            {
                Assert.IsNotNull(actual.ErrorMessage, "ErrorMessage: " + message);
                Assert.IsNull(actual.Label, "Label: " + message);
            }
            else
            {
                Assert.IsNull(actual.ErrorMessage, "ErrorMessage: " + message);
                LabelTest.Check(expectedLabel, actual.Label, "Label: " + message);
            }

            CheckInstruction(actual, expectedMnemonic, "Instruction: " + message);
        }

        private void CheckInstruction(Line actual, String expectedMnemonic, String message)
        {
            Assert.AreEqual(expectedMnemonic, actual.Instruction.Mnemonic, message);
        }

        /// <summary>
        /// ExpandMacro メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ExpandMacro()
        {
            CheckExpandMacro(
                "LBL001 RPUSH",
                TestUtils.MakeArray(
                    MakeGeneratedLine("LBL001", "PUSH", "0,GR1"),
                    MakeGeneratedLine("", "PUSH", "0,GR2"),
                    MakeGeneratedLine("", "PUSH", "0,GR3"),
                    MakeGeneratedLine("", "PUSH", "0,GR4"),
                    MakeGeneratedLine("", "PUSH", "0,GR5"),
                    MakeGeneratedLine("", "PUSH", "0,GR6"),
                    MakeGeneratedLine("", "PUSH", "0,GR7")),
                "マクロ命令 => 内容が展開される");

            const String DcInstructionText = " DC 123";
            CheckExpandMacro(
                DcInstructionText,
                TestUtils.MakeArray(DcInstructionText),
                "マクロ以外の命令 => そのまま");

            const String CommentText = "; コメント行";
            CheckExpandMacro(
                CommentText,
                TestUtils.MakeArray(CommentText),
                "コメント行 => そのまま");
        }

        private void CheckExpandMacro(String text, String[] expected, String message)
        {
            Line target = Line.Parse(text);
            IEnumerable<Line> result = target.ExpandMacro();
            LineCollectionTest.Check(result, expected, message);
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            CheckGenerateLiteralDc(
                " LD GR0,='ABC',GR1",
                MakeGeneratedLine("LTRL0001", "DC", "'ABC'"),
                "オペランドにリテラルを含む命令 => リテラルの定数をオペランドとする DC 命令が生成される");
            CheckGenerateLiteralDc(
                " LD GR0,1234,GR1",
                null,
                "オペランドにリテラルを含まない命令 => DC 命令は生成されない");
            CheckGenerateLiteralDc(
                "; コメント行",
                null,
                "コメント行 => DC 命令は生成されない");
        }

        private void CheckGenerateLiteralDc(String text, String expected, String message)
        {
            Line target = Line.Parse(text);
            LabelTable lblTable = new LabelTable();
            Line generatedLine = target.GenerateLiteralDc(lblTable);
            if (generatedLine == null)
            {
                Assert.IsNull(expected, message);
            }
            else
            {
                String actual = generatedLine.Text;
                Assert.AreEqual(expected, actual, message);
            }
        }

        /// <summary>
        /// SetLabelOffset メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void SetLabelOffset()
        {
            Line instructionLine = Line.Parse("LBL001 LD GR1,GR2");
            LabelTable lblTable = new LabelTable();
            instructionLine.RegisterLabel(lblTable);

            MemoryOffset offsetSet = new MemoryOffset(0xABCD);
            instructionLine.SetLabelOffset(lblTable, offsetSet);

            LabelDefinition labelDef = lblTable.GetDefinitionFor(instructionLine.Label);
            MemoryOffset offsetGot = labelDef.RelOffset;
            MemoryOffsetTest.Check(
                offsetSet, offsetGot, "SetLabelOffset() で設定したラベルのオフセットが取得できる");
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            Line instructionLine = Line.Parse(" LD GR1,GR2");
            CheckGenerateCode(
                instructionLine, WordTest.MakeArray(0x1412),
                "命令行 => その命令のコードを生成する");

            Line commentLine = Line.Parse("; コメント行");
            CheckGenerateCode(
                commentLine, WordTest.MakeArray(),
                "コメント行 => コードを生成しない");

            Line errorLine = Line.Parse(" 未定義命令");
            CheckGenerateCode(
                errorLine, WordTest.MakeArray(),
                "エラー行 => コードを生成しない");
        }

        private void CheckGenerateCode(Line target, Word[] expectedWords, String message)
        {
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(relModule);
            RelocatableModuleTest.CheckWords(relModule, expectedWords, message);
        }

        internal static String MakeGeneratedLine(
            String labelField, String instructionField, String operandField)
        {
            return String.Format("{0}\t{1}\t{2}", labelField, instructionField, operandField);
        }
    }
}
