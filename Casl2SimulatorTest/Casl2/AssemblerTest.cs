using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

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

        /// <summary>
        /// ProcessSourceText で、重複したラベルが定義されている場合。
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
            Assembler target = new Assembler();
            try
            {
                Boolean result = target.ProcessSourceText(sourceText);
                Assert.IsTrue(result, "処理に成功する");
                Assert.IsNotNull(expectedProcessedText, message);
                LineCollectionTest.Check(target.ProcessedLines, expectedProcessedText, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedProcessedText, message);
            }
        }

        /// <summary>
        /// Assemble メソッドのコード生成のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_GenerateCode()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       LD GR6,LBL001,GR7",
                    "LBL001 DC #1234",
                    "       END"),
                WordTest.MakeArray(
                    0x1067,             // OP: 0x10, r/r1: 6, x/r2: 7 => 0x1067 
                    0x0002,             // LBL001 のオフセット
                    0x1234),            // DC で指定した 16 進定数の値
                "再配置可能モジュールにコードとラベル情報が生成される");

            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "LBL001 DC -9876",
                    "LBL002 DC #2468",
                    "LBL003 DC 'ABCｱｲｳ'",
                    "LBL004 DC LBL002",
                    "       END"),
                WordTest.MakeArray(
                    0xD96c,             // -9876 => 0xD96C
                    0x2468,             // #2468 => 0x2468
                    0x0041,             // 'A' => 0x0041
                    0x0042,             // 'B' => 0x0042
                    0x0043,             // 'C' => 0x0043
                    0x00B1,             // 'ｱ' => 0x00B1
                    0x00B2,             // 'ｲ' => 0x00B2
                    0x00B3,             // 'ｳ' => 0x00B3
                    0x0001),            // LBL002 のオフセット
                "10 進定数, 16 進定数, 文字定数, アドレス定数のそれぞれの定数のコード生成");
        }

        /// <summary>
        /// Assemble メソッドでコードのサイズが主記憶より大きい場合のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_ProgramTooBig()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       DS 65535",
                    "       DS 1",
                    "       END"),
                null,
                "コードのサイズが主記憶より大きい => 例外");
        }

        /// <summary>
        /// Assemble メソッドでそれぞれの機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "; 0x00 ~",
                    "       NOP",
                    "; 0x10 ~",
                    "       LD  GR1,#1234,GR2",
                    "       ST  GR3,#2345,GR4",
                    "       LAD GR5,#3456",
                    "       LD  GR6,GR7",
                    "       END"),
                WordTest.MakeArray(
                    0x0000,                 // NOP
                    0x1012, 0x1234,         // LD  r,adr,x
                    0x1134, 0x2345,         // ST  r,adr,x
                    0x1250, 0x3456,         // LOD r,adr,x
                    0x1467),                // LD  r1,r2
                "それぞれの機械語命令");
        }

        private void CheckAssemble(String[] sourceText, Word[] expectedWords, String message)
        {
            Assembler target = new Assembler();
            Boolean notUsed = target.ProcessSourceText(sourceText);
            try
            {
                RelocatableModule relModule = target.Assemble();
                Assert.IsNotNull(expectedWords, message);
                RelocatableModuleTest.Check(relModule, expectedWords, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedWords, message);
            }
        }
    }
}
