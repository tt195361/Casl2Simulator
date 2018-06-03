using System;
using System.Collections;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// <see cref="Word"/> のコレクションです。
    /// </summary>
    internal class WordCollection : IEnumerable<Word>
    {
        #region Static Fields
        private static String m_indexerArgDscr;
        #endregion

        #region Instance Fields
        private readonly List<Word> m_wordList;
        #endregion

        internal WordCollection()
        {
            m_wordList = new List<Word>();
        }

        internal Word this[MemoryOffset offset]
        {
            get
            {
                UInt16 index = offset.Value;
                CheckIndex(index);
                return m_wordList[index];
            }
            set
            {
                UInt16 index = offset.Value;
                CheckIndex(index);
                m_wordList[index] = value;
            }
        }

        private void CheckIndex(UInt16 index)
        {
            if (m_indexerArgDscr == null)
            {
                m_indexerArgDscr = String.Format(
                    Resources.STR_IndexerArgDscr, nameof(WordCollection), nameof(index));
            }

            ArgChecker.CheckRange(index, 0, I32Count - 1, m_indexerArgDscr);
        }

        public IEnumerator<Word> GetEnumerator()
        {
            return m_wordList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(Word word)
        {
            DoAdd(word);
        }

        internal void Add(Word word, Int32 count)
        {
            count.Times(() => DoAdd(word));
        }

        internal void Add(IEnumerable<Word> words)
        {
            words.ForEach((word) => DoAdd(word));
        }

        private void DoAdd(Word word)
        {
            if (Comet2Defs.MemorySize <= I32Count)
            {
                String message = String.Format(
                    Resources.MSG_CouldNotAddToWordCollection, Comet2Defs.MemorySize);
                throw new Casl2SimulatorException(message);
            }

            m_wordList.Add(word);
        }

        internal MemoryOffset GetOffset()
        {
            UInt16 ui16Count = NumberUtils.ToUInt16(I32Count);
            return new MemoryOffset(ui16Count);
        }

        /// <summary>
        /// このコレクションに含まれる語のサイズを取得します。
        /// </summary>
        /// <returns>
        /// このコレクションに含まれる語のサイズを表わす <see cref="MemorySize"/> のオブジェクトを返します。
        /// </returns>
        internal MemorySize GetSize()
        {
            UInt16 ui16Count = NumberUtils.ToUInt16(I32Count);
            return new MemorySize(ui16Count);
        }

        private Int32 I32Count
        {
            get { return m_wordList.Count; }
        }
    }
}
