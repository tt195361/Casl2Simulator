using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="SourceTextProcessor"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class SourceTextProcessorTest
    {
        /// <summary>
        /// <see cref="SourceTextProcessor.ProcessSourceText"/> のマクロ命令の展開の確認。
        /// </summary>
        [TestMethod]
        public void ProcessSourceText_ExpandMacro()
        {
            CheckProcessSourceText(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "LBL001 RPUSH   ; マクロ命令",
                    "       DC 123  ; マクロでない命令",
                    "; コメント行",
                    "       END"),
                TestUtils.MakeArray(
                    "ENTRY  START",
                    ProgramLineTest.MakeGeneratedLine("LBL001", "PUSH", "0,GR1"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR2"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR3"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR4"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR5"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR6"),
                    ProgramLineTest.MakeGeneratedLine("", "PUSH", "0,GR7"),
                    "       DC 123  ; マクロでない命令",
                    "; コメント行",
                    "       END"),
                "マクロ命令が展開された内容に置き換えられる。マクロ命令以外はそのまま");
        }

        /// <summary>
        /// <see cref="SourceTextProcessor.ProcessSourceText"/> のリテラルの DC 命令生成の確認。
        /// </summary>
        [TestMethod]
        public void ProcessSourceText_GenerateLiteralDc()
        {
            CheckProcessSourceText(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       LD GR0,=1234,GR1",
                    "       LD GR2,=#ABCD,GR3",
                    "       PUSH ='!@#$%',GR4",
                    "       END"),
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       LD GR0,=1234,GR1",
                    "       LD GR2,=#ABCD,GR3",
                    "       PUSH ='!@#$%',GR4",
                    ProgramLineTest.MakeGeneratedLine("LTRL0001", "DC", "1234"),
                    ProgramLineTest.MakeGeneratedLine("LTRL0002", "DC", "#ABCD"),
                    ProgramLineTest.MakeGeneratedLine("LTRL0003", "DC", "'!@#$%'"),
                    "       END"),
                "リテラルの定数をオペランドとする DC 命令が生成され、END 命令の直前に配置される");
        }

        /// <summary>
        /// <see cref="SourceTextProcessor.ProcessSourceText"/> で、
        /// リテラルの DC 命令のラベルがプログラムのものと重なる場合。
        /// </summary>
        [TestMethod]
        public void ProcessSourceText_LiteralDcUniqueLabel()
        {
            CheckProcessSourceText(
                TestUtils.MakeArray(
                    "LTRL0001 START",
                    "         LD GR0,=-9876,GR1",
                    "         END"),
                TestUtils.MakeArray(
                    "LTRL0001 START",
                    "         LD GR0,=-9876,GR1",
                    ProgramLineTest.MakeGeneratedLine("LTRL0002", "DC", "-9876"),
                    "         END"),
                "リテラルの DC 命令のラベルはプログラムのものと重ならない一意のものが生成される");
        }

        /// <summary>
        /// <see cref="SourceTextProcessor.ProcessSourceText"/> で、重複したラベルが定義されている場合。
        /// </summary>
        [TestMethod]
        public void ProcessSourceText_DuplicateLabels()
        {
            CheckProcessSourceText(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "LBL001 DC 4660",
                    "LBL001 DC #1234",
                    "       END"),
                null,
                "重複したラベル => 例外");
        }

        private void CheckProcessSourceText(
            String[] sourceText, String[] expectedProcessedText, String message)
        {
            LabelTable lblTable = new LabelTable();
            try
            {
                IEnumerable<ProgramLine> processedLines = SourceTextProcessor.Process(sourceText, lblTable);
                Assert.IsNotNull(expectedProcessedText, message);
                ProgramLineTest.CheckProgramLines(processedLines, expectedProcessedText, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedProcessedText, message);
            }
        }
    }
}
