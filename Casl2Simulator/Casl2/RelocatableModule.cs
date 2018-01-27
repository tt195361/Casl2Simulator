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
        // このモジュールに含まれるコードの語。
        private readonly List<Word> m_codeWords;

        // 実行開始アドレス
        private ExecStartAddress m_execStartAddress;

        // このモジュールが定義し、外部モジュールから参照できるアドレス。
        private ExportLabel m_exportLabel;

        // このモジュールが参照し、外部モジュールが提供するラベルのアドレスを格納する語の情報。
        private readonly List<ImportLabel> m_importLabels;

        // このモジュールの配置アドレスを決めたとき、それに応じて値を更新する語の情報。
        private readonly List<Relocation> m_relocations;
        #endregion

        internal RelocatableModule()
        {
            m_codeWords = new List<Word>();
            m_execStartAddress = null;
            m_exportLabel = null;
            m_importLabels = new List<ImportLabel>();
            m_relocations = new List<Relocation>();
        }

        internal void SetExecStartAddress(ExecStartAddress execStartAddress)
        {
            m_execStartAddress = execStartAddress;
        }

        internal ExecStartAddress ExecStartAddress
        {
            get { return m_execStartAddress; }
        }

        internal void SetExportLabel(ExportLabel exportLabel)
        {
            m_exportLabel = exportLabel;
        }

        internal ExportLabel ExportLabel
        {
            get { return m_exportLabel; }
        }

        internal IEnumerable<ImportLabel> ImportLabels
        {
            get { return m_importLabels; }
        }

        internal IEnumerable<Relocation> Relocations
        {
            get { return m_relocations; }
        }

        internal UInt16 GetCodeOffset()
        {
            return (UInt16)m_codeWords.Count;
        }

        /// <summary>
        /// 指定の語をコードに追加します。
        /// </summary>
        internal void AddWord(Word word)
        {
            m_codeWords.Add(word);
        }

        /// <summary>
        /// 指定のラベルの参照先をコードに追加します。
        /// </summary>
        internal void AddReferenceWord(LabelManager lblManager, Label label)
        {
            // アセンブラは、未定義ラベル (オペランド欄に記述されたラベルのうち、そのプログラム内で
            // 定義されていないラベル) を、他のプログラムの入口名 (START 命令のラベル) と解釈する。
            if (lblManager.IsRegistered(label))
            {
                AddRelocationWord(lblManager, label);
            }
            else
            {
                AddImportLabelWord(label);
            }
        }

        private void AddRelocationWord(LabelManager lblManager, Label label)
        {
            Relocation relocation = new Relocation();
            relocation.AddRelocationWord(this, lblManager, label);
            m_relocations.Add(relocation);
        }

        private void AddImportLabelWord(Label label)
        {
            ImportLabel importLabel = new ImportLabel();
            importLabel.AddImportLabelWord(this, label);
            m_importLabels.Add(importLabel);
        }

        internal Word[] GetCodeWords()
        {
            return m_codeWords.ToArray();
        }
    }
}
