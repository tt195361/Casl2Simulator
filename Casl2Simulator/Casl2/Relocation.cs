using System;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// リンク時にモジュールの配置アドレスを決めたとき、それに応じて値を更新する語の情報を格納します。
    /// </summary>
    internal class Relocation
    {
        #region Fields
        private MemoryOffset m_codeOffset;
        #endregion

        internal Relocation()
        {
            //
        }

        internal MemoryOffset CodeOffset
        {
            get { return m_codeOffset; }
        }

        /// <summary>
        /// リンク時に再配置する語を再配置可能モジュールに追加する。
        /// </summary>
        internal void AddRelocationWord(RelocatableModule relModule, LabelManager lblManager, Label label)
        {
            // 再配置可能モジュールのコードの位置を記録する。
            m_codeOffset = relModule.CodeOffset;

            // 再配置可能モジュールに、コードの語としてラベルのオフセットを追加する。
            MemoryOffset labelOffset = lblManager.GetOffset(label);
            Word codeWord = new Word(labelOffset.Value);
            relModule.AddWord(codeWord);
        }
    }
}
