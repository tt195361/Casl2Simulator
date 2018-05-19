using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AsmStartInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmStartInstructionTest
    {
        #region Instance Fields
        private RelocatableModule m_relModule;
        private Label m_definedLabel;
        private Label m_execStartLabel;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_relModule = new RelocatableModule();
            m_definedLabel = new Label("ENTRY");
            m_execStartLabel = new Label("EXECSTRT");
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
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
            if (success)
            {
                Label actual = target.ExecStartAddress.Label;
                LabelTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// <see cref="AsmStartInstruction.IsStart"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            AsmStartInstruction target = new AsmStartInstruction();
            Boolean result = target.IsStart();
            Assert.IsTrue(result, "START 命令の IsStart() => true");
        }

        /// <summary>
        /// <see cref="AsmStartInstruction.GenerateCode"/> メソッドで、
        /// その行に定義されたラベルがない場合のテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode_NoDefinedLabel()
        {
            ExecStartAddress dontCare = ExecStartAddress.MakeForUnitTest(m_execStartLabel);
            CheckGenerateCode(null, dontCare, false, "定義されたラベルなし => 例外");
        }

        /// <summary>
        /// <see cref="AsmStartInstruction.GenerateCode"/> メソッドで、
        /// その行に定義されたラベルがあり、実行開始番地のラベルが空の場合のテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode_WithDefinedLabel_NoExecStartLabel()
        {
            ExecStartAddress execStartAddress = ExecStartAddress.MakeForUnitTest(null);
            CheckGenerateCode(m_definedLabel, execStartAddress, true, "実行開始番地のラベルが空");
            CheckEntryPoint(
                m_definedLabel, m_definedLabel,
                "ExecStartLabel と ExportLabel の両方に START 命令に定義したラベルが設定される");
        }

        /// <summary>
        /// <see cref="AsmStartInstruction.GenerateCode"/> メソッドで、
        /// その行に定義されたラベルがあり、実行開始番地のラベルを指定した場合のテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode_WithDefinedLabel_WithExecStartLabel()
        {
            ExecStartAddress execStartAddress = ExecStartAddress.MakeForUnitTest(m_execStartLabel);
            CheckGenerateCode(m_definedLabel, execStartAddress, true, "実行開始番地のラベルを指定");
            CheckEntryPoint(
                m_execStartLabel, m_definedLabel,
                "ExecStartLabel に 実行開始番地のラベルが、ExportLabel に START 命令に定義したラベルが設定される");
        }

        private void CheckGenerateCode(
            Label definedLabel, ExecStartAddress execStartAddress, Boolean success, String message)
        {
            AsmStartInstruction target = new AsmStartInstruction();
            target.SetExecStartAddressForUnitTest(execStartAddress);
            try
            {
                target.GenerateCode(definedLabel, m_relModule);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        private void CheckEntryPoint(Label expectedExecStartLabel, Label expectedEntryLabel, String message)
        {
            EntryPoint expected = new EntryPoint(expectedExecStartLabel, expectedEntryLabel);
            EntryPoint actual = m_relModule.EntryPoint;
            EntryPointTest.Check(expected, actual, message);
        }
    }
}
