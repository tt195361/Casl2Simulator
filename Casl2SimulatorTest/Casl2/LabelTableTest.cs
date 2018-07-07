using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LabelTable"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelTableTest
    {
        #region Instance Fields
        private LabelTable m_lblTable;

        private Label m_registeredLabel;
        private Label m_unregisteredLabel;
        private LabelDefinition m_registeredLabelDef;

        private Label m_literal1;
        private Label m_literal2;
        private Label m_literal3;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblTable = new LabelTable();

            m_literal1 = new Label("LTRL0001");
            m_literal2 = new Label("LTRL0002");
            m_literal3 = new Label("LTRL0003");
        }

        /// <summary>
        /// <see cref="LabelTable.Register"/> メソッドのテストです。
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
                m_lblTable.Register(label);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="LabelTable.GetDefinitionFor"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetDefinitionFor()
        {
            PrepareRegisteredLabel();

            CheckGetDefinitionFor(m_registeredLabel, true, "登録したラベル => 取得できる");
            CheckGetDefinitionFor(m_unregisteredLabel, false, "登録していないラベル => 例外");
        }

        private void CheckGetDefinitionFor(Label label, Boolean success, String message)
        {
            try
            {
                LabelDefinition labelDef = m_lblTable.GetDefinitionFor(label);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="LabelTable.FindDefinitionFor"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void FindDefinitionFor()
        {
            PrepareRegisteredLabel();

            CheckFindDefinitionFor(
                m_registeredLabel, m_registeredLabelDef, "登録したラベル => 定義が取得できる");
            CheckFindDefinitionFor(
                m_unregisteredLabel, null, "登録していないラベル => null が返される");
        }

        private void CheckFindDefinitionFor(Label label, LabelDefinition expected, String message)
        {
            LabelDefinition actual = m_lblTable.FindDefinitionFor(label);
            Assert.AreSame(expected, actual, message);
        }

        /// <summary>
        /// <see cref="LabelTable.MakeLiteralLabel"/> メソッドで、
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
        /// <see cref="LabelTable.MakeLiteralLabel"/> メソッドで、
        /// 重複するラベルが登録されている場合のテストです。
        /// </summary>
        [TestMethod]
        public void MakeLiteralLabel_SomeDuplicateLabel()
        {
            m_lblTable.Register(m_literal1);
            CheckMakeLiteralLabel(m_literal2, "重複しないラベル名が生成される");
        }

        /// <summary>
        /// <see cref="LabelTable.MakeLiteralLabel"/> メソッドで、
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
                m_lblTable.Register(label);
            }
        }

        private void CheckMakeLiteralLabel(Label expected, String message)
        {
            try
            {
                Label actual = m_lblTable.MakeLiteralLabel();
                Assert.IsNotNull(expected, message);
                LabelTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }

        private void PrepareRegisteredLabel()
        {
            m_registeredLabel = new Label("REGED");
            m_unregisteredLabel = new Label("UNREGED");

            m_registeredLabelDef = LabelDefinition.MakeForUnitTest(
                m_registeredLabel, MemoryOffset.Zero, MemoryAddress.Zero);
            m_lblTable.RegisterForUnitTest(m_registeredLabelDef);
        }
    }
}
