﻿using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 再配置可能モジュールです。
    /// </summary>
    internal class RelocatableModule
    {
        #region Instance Fields
        private readonly WordCollection m_words;
        private EntryPoint m_entryPoint;
        private readonly List<LabelReference> m_labelRefs;
        #endregion

        internal RelocatableModule()
        {
            m_words = new WordCollection();
            m_entryPoint = null;
            m_labelRefs = new List<LabelReference>();
        }

        /// <summary>
        /// この再配置可能モジュールに含まれる語のコレクションを取得します。
        /// </summary>
        internal IEnumerable<Word> Words
        {
            get { return m_words; }
        }

        /// <summary>
        /// この再配置可能モジュールの実行開始番地に関する情報を取得します。
        /// </summary>
        internal EntryPoint EntryPoint
        {
            get { return m_entryPoint; }
        }

        internal void SetEntryPoint(EntryPoint entryPoint)
        {
            m_entryPoint = entryPoint;
        }

        /// <summary>
        /// この再配置可能モジュール内のラベルへの参照のコレクションを取得します。
        /// </summary>
        internal IEnumerable<LabelReference> LabelRefs
        {
            get { return m_labelRefs; }
        }

        /// <summary>
        /// 指定の語を追加します。
        /// </summary>
        internal void AddWord(Word word)
        {
            m_words.Add(word);
        }

        /// <summary>
        /// 指定の語を指定の数だけ追加します。
        /// </summary>
        internal void AddWords(Word word, Int32 count)
        {
            m_words.Add(word, count);
        }

        /// <summary>
        /// 指定のラベルを参照する語を追加します。
        /// </summary>
        internal void AddReferenceWord(LabelManager lblManager, Label referringlabel)
        {
            LabelReference labelRef = LabelReference.Make(referringlabel, m_words);
            m_labelRefs.Add(labelRef);
        }
    }
}
