using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// LabelManager クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelManagerTest
    {
        #region Fields
        private LabelManager m_lblManager;
        private Label m_label1;
        private Label m_label2;
        private Label m_literal1;
        private Label m_literal2;
        private Label m_literal3;

        private const UInt16 RegisteredOffset = 0x1234;
        private const UInt16 NotRegisteredOffset = 0x0000;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
            m_label1 = new Label("LBL001");
            m_label2 = new Label("LBL002");
            m_literal1 = new Label("LTRL0001");
            m_literal2 = new Label("LTRL0002");
            m_literal3 = new Label("LTRL0003");
        }

        /// <summary>
        /// RegisterLabel メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void RegisterLabel()
        {
            CheckRegisterLabel(m_label1, true, "まだ登録されていないラベル => OK");
            CheckRegisterLabel(m_label1, false, "すでに登録されているラベル => 例外");
        }

        private void CheckRegisterLabel(Label label, Boolean success, String message)
        {
            try
            {
                m_lblManager.RegisterLabel(label);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// SetOffset メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void SetOffset()
        {
            m_lblManager.RegisterLabel(m_label1);

            CheckSetOffset(m_label1, true, "登録されたラベル => オフセットを設定できる");
            CheckSetOffset(m_label2, false, "登録されていないラベル => 例外");
        }

        private void CheckSetOffset(Label label, Boolean success, String message)
        {
            try
            {
                m_lblManager.SetOffset(label, RegisteredOffset);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// GetOffset メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetOffset()
        {
            m_lblManager.RegisterLabel(m_label1);
            m_lblManager.SetOffset(m_label1, RegisteredOffset);

            CheckGetOffset(m_label1, RegisteredOffset, "登録されたラベル => 設定したオフセット");
            CheckGetOffset(m_label2, NotRegisteredOffset, "登録されていないラベル => 0");
        }

        private void CheckGetOffset(Label label, UInt16 expected, String message)
        {
            UInt16 actual = m_lblManager.GetOffset(label);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// MakeLiteralLabel メソッドで、重複するラベルが登録されていない場合のテストです。
        /// </summary>
        [TestMethod]
        public void MakeLiteralLabel_NoDuplicateLabel()
        {
            // 番号順にラベル名が生成される。
            CheckMakeLiteralLabel(m_literal1, "最初のラベル => 'LTRL0001'");
            CheckMakeLiteralLabel(m_literal2, "2 番目のラベル => 'LTRL0002'");
            CheckMakeLiteralLabel(m_literal3, "3 番目のラベル => 'LTRL0003'");
        }

        /// <summary>
        /// MakeLiteralLabel メソッドで、重複するラベルが登録されている場合のテストです。
        /// </summary>
        [TestMethod]
        public void MakeLiteralLabel_SomeDuplicateLabel()
        {
            m_lblManager.RegisterLabel(m_literal1);
            CheckMakeLiteralLabel(m_literal2, "重複しないラベル名が生成される");
        }

        /// <summary>
        /// MakeLiteralLabel メソッドで、リテラルで使用するラベル名がすべて登録されている場合のテストです。
        /// </summary>
        [TestMethod]
        public void MakeLiteralLabel_AllNamesRegistered()
        {
            RegisterAllLiteralLabelNames();
            CheckMakeLiteralLabel(null, "すべての名前が登録されている => 例外");
        }

        private void RegisterAllLiteralLabelNames()
        {
            for (Int32 number = Label.MinLiteralLabelNumber; number <= Label.MaxLiteralLabelNumber; ++number)
            {
                String name = String.Format("LTRL{0:d04}", number);
                Label label = new Label(name);
                m_lblManager.RegisterLabel(label);
            }
        }

        private void CheckMakeLiteralLabel(Label expected, String message)
        {
            try
            {
                Label actual = m_lblManager.MakeLiteralLabel();
                Assert.IsNotNull(expected, message);
                LabelTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }
    }
}
