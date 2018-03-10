using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="RelocatableModule"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RelocatableModuleTest
    {
        #region Instance Fields
        private RelocatableModule m_relModule;
        private LabelManager m_lblManager;
        private Label m_label;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_relModule = new RelocatableModule();
            m_lblManager = new LabelManager();
            m_label = new Label("LBL001");
        }

        /// <summary>
        /// AddWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void AddWord()
        {
            Enumerable.Repeat(0, 65535)
                      .ForEach((notUsed) => CheckAddWord(true, "1..65535 語 => OK"));
            CheckAddWord(false, "65536 語 => 例外");
        }

        private void CheckAddWord(Boolean success, String message)
        {
            try
            {
                m_relModule.AddWord(Word.Zero);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// AddReferenceWord メソッドで指定のラベルが登録されている場合のテストです。
        /// </summary>
        [TestMethod]
        public void AddReferenceWord_RegisteredLabel()
        {
            MemoryOffset DontCareLabelOffset = new MemoryOffset(0x2468);
            m_lblManager.RegisterForUnitTest(m_label, DontCareLabelOffset);
            CheckAddReferenceWord(1, 0, "登録されたラベル => Relocations に追加される");
        }

        /// <summary>
        /// AddReferenceWord メソッドで指定のラベルが登録されていない場合のテストです。
        /// </summary>
        [TestMethod]
        public void AddReferenceWord_NotRegisteredLabel()
        {
            CheckAddReferenceWord(0, 1, "登録されていないラベル => ImportLabels に追加される");
        }

        private void CheckAddReferenceWord(
            Int32 expectedRelocationsCount, Int32 expectedImpotLabelsCount, String message)
        {
            m_relModule.AddReferenceWord(m_lblManager, m_label);

            Int32 actualRelocationsCount = m_relModule.Relocations.Count();
            Assert.AreEqual(expectedRelocationsCount, actualRelocationsCount, "Relocations: " + message);

            Int32 actualImpotLabelsCount = m_relModule.ImportLabels.Count();
            Assert.AreEqual(expectedImpotLabelsCount, actualImpotLabelsCount, "ImportLabels: " + message);
        }

        internal static void Check(RelocatableModule relModule, Word[] expectedCodeWords, String message)
        {
            Word[] actualCodeWords = relModule.GetCodeWords();
            TestUtils.CheckEnumerable(expectedCodeWords, actualCodeWords, WordTest.Check, message);
        }
    }
}
