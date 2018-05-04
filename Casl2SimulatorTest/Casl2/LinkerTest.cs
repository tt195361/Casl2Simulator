using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Linker"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LinkerTest
    {
        #region Fields
        private Linker m_linker;
        private RelocatableModule m_subRelModule;
        private RelocatableModule m_mainRelModule;
        private RelocatableModule[] m_relModules;

        private static readonly String[] SubProgramSourceText = TestUtils.MakeArray(
            "SUB     START ADD1234",
            "ADDEND  DC    #1234",
            "ADD1234 ADDA  GR1,ADDEND",
            "        RET",
            "        END");
        private static readonly String[] MainProgramSourceText = TestUtils.MakeArray(
            "MAIN    START",
            "LBL101  LD    GR1,=3456",
            "        CALL  SUB",
            "        RET",
            "        END");

        private const UInt16 SubProgramBaseAddress = 0x0000;
        private const UInt16 MainProgramBaseAddress = 0x0004;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_linker = new Linker();
            m_subRelModule = MakeRelModule(SubProgramSourceText);
            m_mainRelModule = MakeRelModule(MainProgramSourceText);
            m_relModules = new RelocatableModule[] { m_subRelModule, m_mainRelModule };
        }

        /// <summary>
        /// <see cref="Linker.Link"/> メソッドで各再配置可能モジュールのラベルテーブルの各ラベル定義に
        /// 絶対アドレスが割り当てられること。
        /// </summary>
        [TestMethod]
        public void Link_AssignLabelAddress()
        {
            ExecutableModule notUsed = m_linker.Link(m_relModules);

            CheckAssignedLabelAddress(
                TestUtils.MakeArray(
                    LabelDefinition.MakeForUnitTest("SUB", 0x0000, SubProgramBaseAddress + 0),
                    LabelDefinition.MakeForUnitTest("ADDEND", 0x0000, SubProgramBaseAddress + 0),
                    LabelDefinition.MakeForUnitTest("ADD1234", 0x0001, SubProgramBaseAddress + 1)),
                m_subRelModule, "SUB プログラムのラベル");
            CheckAssignedLabelAddress(
                TestUtils.MakeArray(
                    LabelDefinition.MakeForUnitTest("MAIN", 0x0000, MainProgramBaseAddress + 0),
                    LabelDefinition.MakeForUnitTest("LBL101", 0x0000, MainProgramBaseAddress + 0),
                    LabelDefinition.MakeForUnitTest("LTRL0001", 0x0005, MainProgramBaseAddress + 5)),
                m_mainRelModule, "MAIN プログラムのラベル。'=3456' のリテラルのラベルが生成される");
        }

        private void CheckAssignedLabelAddress(
            LabelDefinition[] expectedLabelDefs, RelocatableModule relModule, String message)
        {
            IEnumerable<LabelDefinition> actualLabelDefs = relModule.LabelTable.LabelDefinitions;
            TestUtils.CheckEnumerable(expectedLabelDefs, actualLabelDefs, LabelDefinitionTest.Check, message);
        }

        private RelocatableModule MakeRelModule(String[] sourceText)
        {
            Assembler asm = new Assembler();
            Boolean notUsed = asm.Assemble(sourceText);
            return asm.RelModule;
        }
    }
}
