using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AsmStartInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmStartInstructionTest
    {
        #region Fields
        private LabelManager m_lblManager;
        private RelocatableModule m_relModule;
        private Label m_execStartLabel;
        private ExecStartAddress m_execStartAddress;

        private readonly MemoryOffset ExecStartOffset = new MemoryOffset(0xABCD);
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
            m_relModule = new RelocatableModule();

            m_execStartLabel = new Label("EXECSTRT");
            m_lblManager.RegisterForUnitTest(m_execStartLabel, ExecStartOffset);
            m_execStartAddress = ExecStartAddress.MakeForUnitTest(m_execStartLabel);
        }

        /// <summary>
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            const Label DontCare = null;

            CheckReadOperand(
                String.Empty, true, null,
                "空文字列 => OK, 実行開始アドレスは空");
            CheckReadOperand(
                LabelTest.ValidLabelName, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, 実行開始アドレスは指定のラベル");
            CheckReadOperand(
                "OPR1,OPR2", false, DontCare,
                "オペランドが 1 より多い => 例外");
        }

        private void CheckReadOperand(String text, Boolean success, Label expected, String message)
        {
            AsmStartInstruction target = new AsmStartInstruction();
            InstructionTest.CheckReadOperand(target, text, success, message);
            if (success)
            {
                Label actual = target.ExecStartAddress.Label;
                LabelTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            AsmStartInstruction target = new AsmStartInstruction();
            Boolean result = target.IsStart();
            Assert.IsTrue(result, "START 命令の IsStart() => true");
        }

        /// <summary>
        /// GenerateCode メソッドで、ラベルがない場合のテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode_NoLabel()
        {
            CheckGenerateCode(null, false, "ラベルなし => 例外");
        }

        /// <summary>
        /// GenerateCode メソッドで、ラベルを指定した場合のテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode_WithLabel()
        {
            Label entryLabel = new Label("ENTRY");
            CheckGenerateCode(entryLabel, true, "ラベルを指定");

            CheckExecStartAddress(
                m_execStartLabel, ExecStartOffset,
                "ExecStartAddress に 実行開始番地のラベルと開始オフセットが設定される");
            CheckExportLabel(
                entryLabel, ExecStartOffset,
                "ExportLabel に START 命令のラベルと開始オフセットが設定される");
        }

        private void CheckGenerateCode(Label entryLabel, Boolean success, String message)
        {
            AsmStartInstruction target = AsmStartInstruction.MakeForUnitTest(m_execStartAddress);
            try
            {
                target.GenerateCode(entryLabel, m_lblManager, m_relModule);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        private void CheckExecStartAddress(Label expectedLabel, MemoryOffset expectedCodeOffset, String message)
        {
            ExecStartAddress expected = ExecStartAddress.MakeForUnitTest(expectedLabel, expectedCodeOffset);
            ExecStartAddress actual = m_relModule.ExecStartAddress;
            ExecStartAddressTest.Check(expected, actual, message);
        }

        private void CheckExportLabel(Label expectedLabel, MemoryOffset expectedCodeOffset, String message)
        {
            ExportLabel expected = new ExportLabel(expectedLabel, expectedCodeOffset);
            ExportLabel actual = m_relModule.ExportLabel;
            ExportLabelTest.Check(expected, actual, message);
        }
    }
}
