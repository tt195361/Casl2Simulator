using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// ExecStartAddress クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ExecStartAddressTest
    {
        #region Fields
        private LabelManager m_lblManager;
        private RelocatableModule m_relModule;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
            m_relModule = new RelocatableModule();
        }

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Label DontCare = null;

            CheckParse(
                String.Empty, true, null,
                "空文字列 => OK, ラベルなし");
            CheckParse(
                LabelTest.ValidLabelName, true, LabelTest.ValidLabel,
                "オペランド 1 つ、有効なラベル => OK, ラベルを指定");
            CheckParse(
                "123", false, DontCare,
                "オペランド 1 つ、ラベル以外 => 例外");
            CheckParse(
                LabelTest.ValidLabelName + ",OPR2", true, LabelTest.ValidLabel,
                "オペランドが 1 より多い => ここでは OK");
        }

        private void CheckParse(String text, Boolean success, Label expected, String message)
        {
            ExecStartAddress target = OperandTest.CheckParse(ExecStartAddress.Parse, text, success, message);
            if (success)
            {
                Label actual = target.Label;
                LabelTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// CalculateCodeOffset メソッドで、ラベルがない場合のテストです。
        /// </summary>
        [TestMethod]
        public void CalculateCodeOffset_NoLabel()
        {
            m_relModule.AddWord(Word.Zero);
            m_relModule.AddWord(Word.Zero);
            m_relModule.AddWord(Word.Zero);
            UInt16 relModuleCodeOffset = m_relModule.GetCodeOffset();
            CheckCalculateCodeOffset(
                null, true, relModuleCodeOffset,
                "ラベルなし => 実行開始オフセットは START の次の命令");
        }

        /// <summary>
        /// CalculateCodeOffset メソッド、定義されたラベルを指定する場合のテストです。
        /// </summary>
        [TestMethod]
        public void CalculateCodeOffset_WithDefinedLabel()
        {
            Label definedLabel = new Label("DEF");
            const UInt16 DefinedLabelOffset = 0x3579;
            m_lblManager.RegisterForUnitTest(definedLabel, DefinedLabelOffset);
            CheckCalculateCodeOffset(
                definedLabel, true, DefinedLabelOffset,
                "定義されたラベル => 実行開始オフセットはラベルのオフセット");
        }

        /// <summary>
        /// CalculateCodeOffset メソッドで、未定義のラベルを指定する場合のテストです。
        /// </summary>
        [TestMethod]
        public void CalculateCodeOffset_WithUndefinedLabel()
        {
            Label undefinedLabel = new Label("UNDEF");
            const UInt16 DontCare = 0;
            CheckCalculateCodeOffset(
                undefinedLabel, false, DontCare,
                "未定義のラベル => 例外");
        }

        private void CheckCalculateCodeOffset(Label label, Boolean success, UInt16 expected, String message)
        {
            ExecStartAddress target = ExecStartAddress.MakeForUnitTest(label);
            try
            {
                target.CalculateCodeOffset(m_lblManager, m_relModule);
                Assert.IsTrue(success, message);
                UInt16 actual = target.CodeOffset;
                Assert.AreEqual(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(
            ExecStartAddress actual, Label expectedLabel, UInt16 expectedCodeOffset, String message)
        {
            LabelTest.Check(expectedLabel, actual.Label, "Label: " + message);
            Assert.AreEqual(expectedCodeOffset, actual.CodeOffset, "CodeOffset: " + message);
        }
    }
}
