using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Assembler クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AssemblerTest
    {
        /// <summary>
        /// ProcessSourceText で、マクロ命令の展開の確認。
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
                    "LBL001\tPUSH\t0,GR1",
                    "\tPUSH\t0,GR2",
                    "\tPUSH\t0,GR3",
                    "\tPUSH\t0,GR4",
                    "\tPUSH\t0,GR5",
                    "\tPUSH\t0,GR6",
                    "\tPUSH\t0,GR7",
                    "       DC 123  ; マクロでない命令",
                    "; コメント行",
                    "       END"),
                "マクロ命令が展開された内容に置き換えられる。マクロ命令以外はそのまま");
        }

        /// <summary>
        /// ProcessSourceText で、リテラルの DC 命令生成の確認。
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
                    "LTRL0001\tDC\t1234",
                    "LTRL0002\tDC\t#ABCD",
                    "LTRL0003\tDC\t'!@#$%'",
                    "       END"),
                "リテラルの定数をオペランドとする DC 命令が生成され、END 命令の直前に配置される");
        }

        /// <summary>
        /// ProcessSourceText で、リテラルの DC 命令のラベルがプログラムのものと重ならず一意になる。
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
                    "LTRL0002\tDC\t-9876",
                    "         END"),
                "リテラルの DC 命令のラベルはプログラムのものと重ならない一意のものが生成される");
        }

        private void CheckProcessSourceText(
            String[] sourceText, String[] expectedProcessedText, String message)
        {
            Assembler target = new Assembler();
            Boolean result = target.ProcessSourceText(sourceText);
            Assert.IsTrue(result, "処理に成功する");
            LineCollectionTest.Check(target.ProcessedLines, expectedProcessedText, message);
        }
    }
}
