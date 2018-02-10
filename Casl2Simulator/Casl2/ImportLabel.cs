using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 外部モジュールが提供するラベルを格納する語の情報を保持します。
    /// </summary>
    internal class ImportLabel
    {
        #region Fields
        private Label m_label;
        private MemoryOffset m_codeOffset;

        private readonly Word PlaceHolderValue = Word.Zero;
        #endregion

        internal ImportLabel()
        {
            //
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal MemoryOffset CodeOffset
        {
            get { return m_codeOffset; }
        }

        internal void AddImportLabelWord(RelocatableModule relModule, Label label)
        {
            // 参照先のラベルと再配置可能モジュールのコードの位置を記録する。
            m_label = label;
            m_codeOffset = relModule.CodeOffset;

            // 再配置可能モジュールに、リンク時に値を入れる場所を確保する。
            relModule.AddWord(PlaceHolderValue);
        }
    }
}
