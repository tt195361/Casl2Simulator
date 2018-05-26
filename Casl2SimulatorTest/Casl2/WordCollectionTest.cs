using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="WordCollection"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class WordCollectionTest
    {
        #region Instance Fields
        private WordCollection m_words;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_words = new WordCollection();
        }

        /// <summary>
        /// <see cref="WordCollection"/> の Indexer の設定、取得する値のテストです。
        /// </summary>
        [TestMethod]
        public void Indexer_Value()
        {
            MemoryOffset Index = MemoryOffset.Zero;
            m_words.Add(Word.Zero);

            Word wordSet = new Word(0x1234);
            m_words[Index] = wordSet;

            Word wordGot = m_words[Index];
            WordTest.Check(wordSet, wordGot, "設定した値が取得できる");
        }

        /// <summary>
        /// <see cref="WordCollection"/> の Indexer のインデックスのテストです。
        /// </summary>
        [TestMethod]
        public void Indexer_Index()
        {
            WordCollection emptyWords = MakeWords(0);
            CheckIndexer_Index(emptyWords, 0, false, "emptyWords[0] => 下限より小さい => 例外");
            CheckIndexer_Index(emptyWords, 1, false, "emptyWords[1] => 上限より大きい => 例外");

            WordCollection threeWords = MakeWords(3);
            CheckIndexer_Index(threeWords, 0, true, "threeWords[0] => ちょうど下限 => OK");
            CheckIndexer_Index(threeWords, 2, true, "threeWords[2] => ちょうど上限 => OK");
            CheckIndexer_Index(threeWords, 3, false, "threeWords[3] => 上限より大きい => 例外");

            WordCollection fullWords = MakeWords(65536);
            CheckIndexer_Index(fullWords, 0, true, "fullWords[0] => ちょうど下限 => OK");
            CheckIndexer_Index(fullWords, 65535, true, "fullWords[65535] => ちょうど上限 => OK");
        }

        private void CheckIndexer_Index(
            WordCollection words, UInt16 ui16Index, Boolean success, String message)
        {
            MemoryOffset index = new MemoryOffset(ui16Index);
            CheckGetIndexer_Index(words, index, success, message);
            CheckSetIndexer_Index(words, index, success, message);
        }

        private void CheckGetIndexer_Index(
                WordCollection words, MemoryOffset index, Boolean success, String message)
        {
            try
            {
                Word notUsed = words[index];
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        private void CheckSetIndexer_Index(
                WordCollection words, MemoryOffset index, Boolean success, String message)
        {
            try
            {
                words[index] = Word.Zero;
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="WordCollection.Add"/> メソッドで指定の語が追加されることのテストです。
        /// </summary>
        [TestMethod]
        public void Add_Value()
        {
            const Int16 One = 1;
            const Int16 Two = 2;
            const Int16 Three = 3;

            CheckAdd_Value(One, WordTest.MakeArray(One), "1 語目");
            CheckAdd_Value(Two, WordTest.MakeArray(One, Two), "2 語目");
            CheckAdd_Value(Three, WordTest.MakeArray(One, Two, Three), "3 語目");
        }

        private void CheckAdd_Value(Int16 value, Word[] expected, String message)
        {
            Word word = new Word(value);
            m_words.Add(word);
            TestUtils.CheckEnumerable(expected, m_words, message);
        }

        /// <summary>
        /// <see cref="WordCollection.Add"/> メソッドのコレクションのサイズのテストです。
        /// </summary>
        [TestMethod]
        public void Add_CollectionSize()
        {
            65536.Times(() => CheckAdd_CollectionSize(true, "65536 語まで => OK"));
            CheckAdd_CollectionSize(false, "65537 語目 => 例外");
        }

        private void CheckAdd_CollectionSize(Boolean success, String message)
        {
            try
            {
                m_words.Add(Word.Zero);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// <see cref="WordCollection.GetOffset"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetOffset()
        {
            CheckGetOffset(0, "コレクションが空 => オフセットは 0");

            m_words.Add(Word.Zero);
            CheckGetOffset(1, "コレクションが 1 語 => オフセットは 1");

            m_words.Add(Word.Zero);
            m_words.Add(Word.Zero);
            CheckGetOffset(3, "コレクションが 3 語 => オフセットは 3");
        }

        private void CheckGetOffset(UInt16 expectedValue, String message)
        {
            MemoryOffset expectedOffset = new MemoryOffset(expectedValue);
            MemoryOffset actualOffset = m_words.GetOffset();
            MemoryOffsetTest.Check(expectedOffset, actualOffset, message);
        }

        internal static WordCollection MakeWords(Int32 count)
        {
            WordCollection words = new WordCollection();
            words.Add(Word.Zero, count);
            return words;
        }
    }
}
