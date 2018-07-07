using System;
using Tt195361.Casl2Simulator.Common;

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
            : this(label, MemoryOffset.Zero, MemoryAddress.Zero)
        {
            //
        }

        private LabelDefinition(Label label, MemoryOffset relOffset, MemoryAddress absAddress)
        {
            m_label = label;
            m_relOffset = relOffset;
            m_absAddress = absAddress;
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

        /// <summary>
        /// 定義したラベルの再配置可能モジュール内のオフセットを設定します。
        /// </summary>
        /// <param name="relOffset">設定する再配置可能モジュール内のオフセットです。</param>
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

        /// <summary>
        /// 定義したラベルに実行可能モジュールでの絶対アドレスを割り当てます。
        /// </summary>
        /// <param name="baseAddress">
        /// ラベルが定義された再配置可能モジュールが実行可能モジュールで配置されるアドレスです。
        /// </param>
        internal void AssignAbsAddress(MemoryAddress baseAddress)
        {
            m_absAddress = baseAddress.Add(m_relOffset);
        }

        public override String ToString()
        {
            return String.Format("{0}: Rel={1}, Abs={2}", m_label, m_relOffset, m_absAddress);
        }

        internal static LabelDefinition MakeForUnitTest(
            Label label, MemoryOffset relOffset, MemoryAddress absAddress)
        {
            return new LabelDefinition(label, relOffset, absAddress);
        }
    }
}
