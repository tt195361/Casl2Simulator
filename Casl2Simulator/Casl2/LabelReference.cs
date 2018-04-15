using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// ラベルへの参照を取り扱います。
    /// </summary>
    internal class LabelReference
    {
        #region Static Fields
        private static readonly Word PlaceHolder = Word.Zero;
        #endregion

        internal static LabelReference Make(Label referringLabel, WordCollection words)
        {
            MemoryOffset wordOffset = words.GetOffset();
            words.Add(PlaceHolder);
            return new LabelReference(referringLabel, wordOffset);
        }

        #region Instance Fields
        private readonly Label m_referringLabel;
        private readonly MemoryOffset m_wordOffset;
        #endregion

        private LabelReference(Label referringLabel, MemoryOffset wordOffset)
        {
            ArgChecker.CheckNotNull(referringLabel, nameof(referringLabel));

            m_referringLabel = referringLabel;
            m_wordOffset = wordOffset;
        }

        /// <summary>
        /// 参照するラベルを取得します。
        /// </summary>
        internal Label ReferringLabel
        {
            get { return m_referringLabel; }
        }

        /// <summary>
        /// 参照するラベルのアドレスが入る語の再配置可能モジュール内のオフセットを取得します。
        /// </summary>
        internal MemoryOffset WordOffset
        {
            get { return m_wordOffset; }
        }

        public override String ToString()
        {
            return String.Format("{0}: {1}", m_referringLabel, m_wordOffset);
        }

        internal static LabelReference MakeForUnitTest(Label referringLabel, MemoryOffset wordOffset)
        {
            return new LabelReference(referringLabel, wordOffset);
        }
    }
}
