using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 他のモジュールに公開するラベルの情報を保持します。
    /// </summary>
    internal class ExportLabel
    {
        #region Fields
        private readonly Label m_label;
        private readonly UInt16 m_codeOffset;
        #endregion

        internal ExportLabel(Label label, UInt16 codeOffset)
        {
            m_label = label;
            m_codeOffset = codeOffset;
        }

        internal Label Label
        {
            get { return m_label; }
        }

        internal UInt16 CodeOffset
        {
            get { return m_codeOffset; }
        }

        public override String ToString()
        {
            return String.Format("{0}: 0x{1:X04}", m_label, m_codeOffset);
        }
    }
}
