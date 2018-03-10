using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="MachineInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MachineInstructionTest
    {
        #region Instance Fields
        private ProgramInstruction m_rAdrXOrR1R2_RAdrX;
        private ProgramInstruction m_rAdrXOrR1R2_R1R2;
        private LabelManager m_lblManager;

        private const String Mnemonic = "TST";
        private const UInt16 Opcode1 = 0x01;
        private const UInt16 Opcode2 = 0x02;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_rAdrXOrR1R2_RAdrX = ProgramInstructionTest.Make(MnemonicDef.LD, "GR1,#ABCD,GR2");
            m_rAdrXOrR1R2_R1R2 = ProgramInstructionTest.Make(MnemonicDef.LD, "GR3,GR4");
            m_lblManager = new LabelManager();
        }

        /// <summary>
        /// オペランドが r,adr,x か r1,r2 の機械語命令の ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand_RAdrXOrR1R2()
        {
            CheckReadOperand_RAdrXOrR1R2("GR3,='ABC',GR5", true, "r,adr,x の場合 => OK");
            CheckReadOperand_RAdrXOrR1R2("GR4,GR6", true, "r1,r2 の場合 => OK");
            CheckReadOperand_RAdrXOrR1R2("GR5,GR6,GR7", false, "後ろにまだ文字がある => 例外");
        }

        private void CheckReadOperand_RAdrXOrR1R2(String text, Boolean success, String message)
        {
            MachineInstruction target = MachineInstruction.MakeRAdrXOrR1R2(Mnemonic, Opcode1, Opcode2);
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// オペランドが r,adr[,x] の機械語命令の ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand_RAdrX()
        {
            CheckReadOperand_RAdrX("GR6,#89AB", true, "r,adr の場合 => OK");
            CheckReadOperand_RAdrX("GR7,LBL001,GR2", true, "r,adr,x の場合 => OK");
            CheckReadOperand_RAdrX("GR0,GR1", false, "r1,r2 は受け付けない => 例外");
            CheckReadOperand_RAdrX("GR1,1234,GR2,5678", false, "後ろにまだ文字がある => 例外");
        }

        private void CheckReadOperand_RAdrX(String text, Boolean success, String message)
        {
            MachineInstruction target = MachineInstruction.MakeRAdrX(Mnemonic, Opcode1);
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// オペランドが adr,x の機械語命令の ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand_AdrX()
        {
            CheckReadOperand_AdrX("='ABC',GR1", true, "adr,x の場合 => OK");
            CheckReadOperand_AdrX("#1357,GR3,GR4", false, "後ろにまだ文字がある => 例外");
        }

        private void CheckReadOperand_AdrX(String text, Boolean success, String message)
        {
            MachineInstruction target = MachineInstruction.MakeAdrX(Mnemonic, Opcode1);
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// オペランドなしの機械語命令の ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand_NoOperand()
        {
            CheckReadOperand_NoOperand(String.Empty, true, "空文字列 => OK");
            CheckReadOperand_NoOperand("OPR", false, "なにか文字がある => 例外");
        }

        private void CheckReadOperand_NoOperand(String text, Boolean success, String message)
        {
            MachineInstruction target = MachineInstruction.MakeNoOperand(Mnemonic, Opcode1);
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            ProgramInstruction target = ProgramInstructionTest.Make(MnemonicDef.LD, "GR1,=1234,GR2");
            LabelManager lblManager = new LabelManager();
            String actual = target.GenerateLiteralDc(lblManager);
            const String Expected = "LTRL0001\tDC\t1234";
            Assert.AreEqual(Expected, actual, "リテラルの DC 命令が生成される");
        }

        /// <summary>
        /// GenerateCode メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(
                m_rAdrXOrR1R2_RAdrX, WordTest.MakeArray(0x1012, 0xabcd),
                "r,adr,x or r1,r2 で r,adr,x の場合: opcode=0x10, r/r1=1, x/r2=2, adr=0xabcd");
            CheckGenerateCode(
                m_rAdrXOrR1R2_R1R2, WordTest.MakeArray(0x1434),
                "r,adr,x or r1,r2 で r1,r2 の場合: opcode=0x14, r/r1=3, x/r2=4, adr=なし");
        }

        private void CheckGenerateCode(ProgramInstruction target, Word[] expectedWords, String message)
        {
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(null, m_lblManager, relModule);
            RelocatableModuleTest.Check(relModule, expectedWords, message);
        }
    }
}
