using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// MachineInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MachineInstructionTest
    {
        #region Fields
        private Instruction m_rAdrXOrR1R2_RAdrX;
        private Instruction m_rAdrXOrR1R2_R1R2;
        private LabelManager m_lblManager;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_rAdrXOrR1R2_RAdrX = InstructionTest.Make(MnemonicDef.LD, "GR1,#ABCD,GR2");
            m_rAdrXOrR1R2_R1R2 = InstructionTest.Make(MnemonicDef.LD, "GR3,GR4");
            m_lblManager = new LabelManager();
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            Instruction target = InstructionTest.Make(MnemonicDef.LD, "GR1,=1234,GR2");
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

        private void CheckGenerateCode(Instruction target, Word[] expectedWords, String message)
        {
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(null, m_lblManager, relModule);
            RelocatableModuleTest.Check(relModule, expectedWords, message);
        }
    }
}
