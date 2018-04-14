﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LabelManager"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelManagerTest
    {
        #region Instance Fields
        private LabelManager m_lblManager;
        private Label m_literal1;
        private Label m_literal2;
        private Label m_literal3;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
            m_literal1 = new Label("LTRL0001");
            m_literal2 = new Label("LTRL0002");
            m_literal3 = new Label("LTRL0003");
        }

        /// <summary>
        /// <see cref="LabelManager.Register"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Register()
        {
            Label label = new Label("LBL001");

            CheckRegister(label, true, "まだ登録されていないラベル => 登録できる");
            CheckRegister(label, false, "すでに登録されているラベル => 例外");
        }

        private void CheckRegister(Label label, Boolean success, String message)
        {
            try
            {
                m_lblManager.Register(label);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="LabelManager.GetDefinitionFor"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetDefinitionFor()
        {
            Label registeredLabel = new Label("REGED");
            Label notRegisteredLabel = new Label("NOTREGED");
            m_lblManager.Register(registeredLabel);

            CheckGetDefinitionFor(registeredLabel, true, "登録したラベル => 取得できる");
            CheckGetDefinitionFor(notRegisteredLabel, false, "登録していないラベル => 例外");
        }

        private void CheckGetDefinitionFor(Label label, Boolean success, String message)
        {
            try
            {
                LabelDefinition labelDef = m_lblManager.GetDefinitionFor(label);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="LabelManager.MakeLiteralLabel"/> メソッドで、
        /// 重複するラベルが登録されていない場合のテストです。
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
        /// <see cref="LabelManager.MakeLiteralLabel"/> メソッドで、
        /// 重複するラベルが登録されている場合のテストです。
        /// </summary>
        [TestMethod]
        public void MakeLiteralLabel_SomeDuplicateLabel()
        {
            m_lblManager.Register(m_literal1);
            CheckMakeLiteralLabel(m_literal2, "重複しないラベル名が生成される");
        }

        /// <summary>
        /// <see cref="LabelManager.MakeLiteralLabel"/> メソッドで、
        /// リテラルで使用するラベル名がすべて登録されている場合のテストです。
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
                m_lblManager.Register(label);
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
