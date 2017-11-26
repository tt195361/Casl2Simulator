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

        private const UInt16 RegisteredOffset = 0x1234;
        private const UInt16 NotRegisteredOffset = 0x0000;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
            m_label1 = new Label("LBL001");
            m_label2 = new Label("LBL002");
        }

        /// <summary>
        /// Register メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Register()
        {
            CheckRegister(m_label1, true, "まだ登録されていないラベル => OK");
            CheckRegister(m_label1, false, "すでに登録されているラベル => 例外");
        }

        private void CheckRegister(Label label, Boolean success, String message)
        {
            try
            {
                m_lblManager.Register(label, RegisteredOffset);
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
            m_lblManager.Register(m_label1, RegisteredOffset);

            CheckGetOffset(m_label1, RegisteredOffset, "登録されたラベル => 登録したオフセット");
            CheckGetOffset(m_label2, NotRegisteredOffset, "登録されていないラベル => 0");
        }

        private void CheckGetOffset(Label label, UInt16 expected, String message)
        {
            UInt16 actual = m_lblManager.GetOffset(label);
            Assert.AreEqual(expected, actual, message);
        }
    }
}
