using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
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
        private Label m_label;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_relModule = new RelocatableModule();
            m_label = new Label("LBL001");
        }

        /// <summary>
        /// <see cref="RelocatableModule.AddReferenceWord"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void AddReferenceWord()
        {
            Word word1 = new Word(0x1234);
            Word word2 = new Word(0x5678);
            m_relModule.AddWord(word1);
            m_relModule.AddWord(word2);
            MemoryOffset wordOffset = new MemoryOffset(2);

            m_relModule.AddReferenceWord(m_label);

            IEnumerable<Word> actualWords = m_relModule.Words;
            Word[] expectedWords = TestUtils.MakeArray(word1, word2, Word.Zero);
            TestUtils.CheckEnumerable(
                expectedWords, actualWords,
                "語のコレクションに、リンク時にラベルのアドレスと置き換わる値が 0 の語が追加される");

            Int32 labelRefsCount = m_relModule.LabelRefs.Count();
            Assert.AreEqual(1, labelRefsCount, "ラベル参照のコレクションに、ラベルへの参照が 1 つ追加される");

            LabelReference expectedLabelRef = LabelReference.MakeForUnitTest(m_label, wordOffset);
            LabelReference actualLabelRef = m_relModule.LabelRefs.First();
            LabelReferenceTest.Check(
                expectedLabelRef, actualLabelRef,
                "ラベルへの参照には、参照するラベルとそのラベルのアドレスが入る語のオフセットが記録される");
        }

        internal static void CheckWords(RelocatableModule relModule, Word[] expectedWords, String message)
        {
            IEnumerable<Word> actualWords = relModule.Words;
            TestUtils.CheckEnumerable(expectedWords, actualWords, WordTest.Check, message);
        }
    }
}
