﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Assembler"/> クラスの単体テストです。
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
                    LineTest.MakeGeneratedLine("LBL001", "PUSH", "0,GR1"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR2"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR3"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR4"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR5"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR6"),
                    LineTest.MakeGeneratedLine("", "PUSH", "0,GR7"),
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
                    LineTest.MakeGeneratedLine("LTRL0001", "DC", "1234"),
                    LineTest.MakeGeneratedLine("LTRL0002", "DC", "#ABCD"),
                    LineTest.MakeGeneratedLine("LTRL0003", "DC", "'!@#$%'"),
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
                    LineTest.MakeGeneratedLine("LTRL0002", "DC", "-9876"),
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
        /// Assemble メソッドでオペコードが 0x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_0x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       NOP",
                    "       END"),
                WordTest.MakeArray(
                    0x0000),                // NOP
                "オペコードが 0x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 1x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_1x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       LD   GR1,#1234,GR2",
                    "       ST   GR3,#2345,GR4",
                    "       LAD  GR5,#3456",
                    "       LD   GR6,GR7",
                    "       END"),
                WordTest.MakeArray(
                    0x1012, 0x1234,         // LD   r,adr,x
                    0x1134, 0x2345,         // ST   r,adr,x
                    0x1250, 0x3456,         // LOD  r,adr,x
                    0x1467),                // LD   r1,r2
                "オペコードが 1x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 2x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_2x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       ADDA GR7,#FEDC,GR6",
                    "       SUBA GR5,#BA98,GR4",
                    "       ADDL GR3,#7654,GR2",
                    "       SUBL GR1,#3210,GR7",
                    "       ADDA GR1,GR2",
                    "       SUBA GR3,GR4",
                    "       ADDL GR5,GR6",
                    "       SUBL GR7,GR1",
                    "       END"),
                WordTest.MakeArray(
                    0x2076, 0xFEDC,         // ADDA r,adr,x
                    0x2154, 0xBA98,         // SUBA r,adr,x
                    0x2232, 0x7654,         // ADDL r,adr,x
                    0x2317, 0x3210,         // SUBL r,adr,x
                    0x2412,                 // ADDA r1,r2
                    0x2534,                 // SUBA r1,r2
                    0x2656,                 // ADDL r1,r2
                    0x2771),                // SUBL r1,r2
                "オペコードが 2x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 3x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_3x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       AND  GR1,#1357,GR3",
                    "       OR   GR5,#9BDF,GR7",
                    "       XOR  GR2,#2468,GR4",
                    "       AND  GR0,GR2",
                    "       OR   GR4,GR6",
                    "       XOR  GR3,GR5",
                    "       END"),
                WordTest.MakeArray(
                    0x3013, 0x1357,         // AND  r,adr,x
                    0x3157, 0x9BDF,         // OR   r,adr,x
                    0x3224, 0x2468,         // XOR  r,adr,x
                    0x3402,                 // AND  r1,r2
                    0x3546,                 // OR   r1,r2
                    0x3635),                // XOR  r1,r2
                "オペコードが 3x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 4x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_4x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       CPA  GR0,#01EF,GR7",
                    "       CPL  GR2,#23CD,GR5",
                    "       CPA  GR1,GR6",
                    "       CPL  GR3,GR4",
                    "       END"),
                WordTest.MakeArray(
                    0x4007, 0x01EF,         // CPA  r,adr,x
                    0x4125, 0x23CD,         // CPL  r,adr,x
                    0x4416,                 // CPA  r1,r2
                    0x4534),                // CPL  r1,r2
                "オペコードが 4x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 5x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_5x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       SLA  GR7,#7654,GR6",
                    "       SRA  GR5,#5432,GR4",
                    "       SLL  GR3,#3210,GR2",
                    "       SRL  GR1,#10FE,GR7",
                    "       END"),
                WordTest.MakeArray(
                    0x5076, 0x7654,         // SLA  r,adr,x
                    0x5154, 0x5432,         // SRA  r,adr,x
                    0x5232, 0x3210,         // SLL  r,adr,x
                    0x5317, 0x10FE),        // SRL  r,adr,x
                "オペコードが 5x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 6x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_6x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       JMI  #1111,GR2",
                    "       JNZ  #2222,GR3",
                    "       JZE  #3333,GR4",
                    "       JUMP #4444,GR5",
                    "       JPL  #5555,GR6",
                    "       JOV  #6666,GR7",
                    "       END"),
                WordTest.MakeArray(
                    0x6102, 0x1111,         // JMI  adr,x
                    0x6203, 0x2222,         // JNZ  adr,x
                    0x6304, 0x3333,         // JZE  adr,x
                    0x6405, 0x4444,         // JUMP adr,x
                    0x6506, 0x5555,         // JPL  adr,x
                    0x6607, 0x6666),        // JOV  adr,x
                "オペコードが 6x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 7x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_7x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       PUSH #89AB,GR5",
                    "       POP  GR6",
                    "       END"),
                WordTest.MakeArray(
                    0x7005, 0x89AB,         // PUSH adr,x
                    0x7160),                // POP  r
                "オペコードが 7x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが 8x の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_8x()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       CALL #9ABC,GR2",
                    "       RET",
                    "       END"),
                WordTest.MakeArray(
                    0x8002, 0x9ABC,         // CALL adr,x
                    0x8100),                // RET
                "オペコードが 8x の機械語命令");
        }

        /// <summary>
        /// Assemble メソッドでオペコードが Fx の機械語命令のテストです。
        /// </summary>
        [TestMethod]
        public void Assemble_MachineInstructions_Fx()
        {
            CheckAssemble(
                TestUtils.MakeArray(
                    "ENTRY  START",
                    "       SVC  #ABCD,GR3",
                    "       END"),
                WordTest.MakeArray(
                    0xF003, 0xABCD),        // CALL adr,x
                "オペコードが Fx の機械語命令");
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
