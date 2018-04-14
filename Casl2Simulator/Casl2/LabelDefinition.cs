using System;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ラベルの定義を表わします。
    /// </summary>
    internal class LabelDefinition
    {
        #region Instance Fields
        private readonly Label m_label;
        private MemoryOffset m_relOffset;
        private MemoryAddress m_absAddress;
        #endregion

        internal LabelDefinition(Label label)
        {
            m_label = label;
            m_relOffset = MemoryOffset.Zero;
            m_absAddress = MemoryAddress.Zero;
        }

        /// <summary>
        /// 定義したラベルを取得します。
        /// </summary>
        internal Label Label
        {
            get { return m_label; }
        }

        /// <summary>
        /// 定義したラベルの再配置可能モジュール内のオフセットを取得します。
        /// </summary>
        internal MemoryOffset RelOffset
        {
            get { return m_relOffset; }
        }

        internal void SetRelOffset(MemoryOffset relOffset)
        {
            m_relOffset = relOffset;
        }

        /// <summary>
        /// 定義したラベルの実行可能モジュールでの絶対アドレスを取得します。
        /// </summary>
        internal MemoryAddress AbsAddress
        {
            get { return m_absAddress; }
        }

        internal void Relocate(MemoryAddress baseAddress)
        {
            m_absAddress = baseAddress.Add(m_relOffset);
        }

        public override String ToString()
        {
            return String.Format("{0}: Rel={1}, Abs={2}", m_label, m_relOffset, m_absAddress);
        }
    }
}
