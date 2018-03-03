using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 他のモジュールに公開するラベルを表わします。
    /// </summary>
    internal class ExportLabel
    {
        #region Instance Fields
        private readonly Label m_label;
        private readonly MemoryOffset m_codeOffset;
        #endregion

        internal ExportLabel(Label label, MemoryOffset codeOffset)
        {
            m_label = label;
            m_codeOffset = codeOffset;
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal MemoryOffset CodeOffset
        {
            get { return m_codeOffset; }
        }

        public override String ToString()
        {
            return String.Format("{0}: {1}", m_label, m_codeOffset);
        }
    }
}
