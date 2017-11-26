using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// アセンブラの出力となる再配置可能モジュールです。
    /// </summary>
    internal class RelocatableModule
    {
        #region Fields
        private readonly List<Word> m_wordList;
        #endregion

        internal RelocatableModule()
        {
            m_wordList = new List<Word>();
        }

        internal Int32 WordCount
        {
            get { return m_wordList.Count; }
        }

        // 実行開始アドレス

        // Exports: このモジュールが定義し、外部モジュールから参照できるアドレス。

        // Imports: このモジュールが参照し、外部モジュールが提供するアドレス。

        // コード

        internal UInt16 GetCurrentOffset()
        {
            return (UInt16)m_wordList.Count;
        }

        internal void AddWord(Word word)
        {
            m_wordList.Add(word);
        }

        internal Word[] GetWords()
        {
            return m_wordList.ToArray();
        }
    }
}
