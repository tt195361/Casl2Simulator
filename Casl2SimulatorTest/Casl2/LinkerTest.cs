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

        private const UInt16 SUB_Address = SubProgramBaseAddress + 0;
        private const UInt16 ADDEND_Address = SubProgramBaseAddress + 0;
        private const UInt16 ADD1234_Address = SubProgramBaseAddress + 1;
        private const UInt16 MAIN_Address = MainProgramBaseAddress + 0;
        private const UInt16 LBL101_Address = MainProgramBaseAddress + 0;
        private const UInt16 LTRL0001_Address = MainProgramBaseAddress + 5;
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
                    LabelDefinitionTest.Make("SUB", 0x0000, SUB_Address),
                    LabelDefinitionTest.Make("ADDEND", 0x0000, ADDEND_Address),
                    LabelDefinitionTest.Make("ADD1234", 0x0001, ADD1234_Address)),
                m_subRelModule, "SUB プログラムのラベル");
            CheckAssignedLabelAddress(
                TestUtils.MakeArray(
                    LabelDefinitionTest.Make("MAIN", 0x0000, MAIN_Address),
                    LabelDefinitionTest.Make("LBL101", 0x0000, LBL101_Address),
                    LabelDefinitionTest.Make("LTRL0001", 0x0005, LTRL0001_Address)),
                m_mainRelModule, "MAIN プログラムのラベル。'=3456' のリテラルのラベルが生成される");
        }

        private void CheckAssignedLabelAddress(
            LabelDefinition[] expectedLabelDefs, RelocatableModule relModule, String message)
        {
            IEnumerable<LabelDefinition> actualLabelDefs = relModule.LabelTable.LabelDefinitions;
            TestUtils.CheckEnumerable(expectedLabelDefs, actualLabelDefs, LabelDefinitionTest.Check, message);
        }

        /// <summary>
        /// <see cref="Linker.Link"/> メソッドで各再配置可能モジュールの EntryPoint が
        /// EntryPointTable に登録されること。
        /// </summary>
        [TestMethod]
        public void Link_RegisterEntryPointsIn()
        {
            ExecutableModule notUsed = m_linker.Link(m_relModules);

            EntryPoint[] expected = TestUtils.MakeArray(
                EntryPointTest.Make("ADD1234", "SUB", ADD1234_Address),
                EntryPointTest.Make("MAIN", "MAIN", MAIN_Address));
            IEnumerable<EntryPoint> actual = m_linker.EntryPointTable.EntryPoints;
            TestUtils.CheckEnumerable(
                expected, actual, EntryPointTest.Check,
                "各再配置可能モジュールの EntryPoint にアドレスが設定され、EntryPointTable に登録される");
        }

        private RelocatableModule MakeRelModule(String[] sourceText)
        {
            Assembler asm = new Assembler();
            Boolean notUsed = asm.Assemble(sourceText);
            return asm.RelModule;
        }
    }
}
