using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II のフラグレジスタ (Flag Register) を表わします。
    /// </summary>
    internal class FlagRegister
    {
        #region Fields
        // 各フラグの型は Boolean にしました。enum の Flags よりも簡単に値を設定できそうだと考えました。
        private Boolean m_overflowFlag;
        private Boolean m_signFlag;
        private Boolean m_zeroFlag;
        #endregion

        /// <summary>
        /// <see cref="FlagRegister"/> のインスタンスを初期化します。
        /// </summary>
        internal FlagRegister()
        {
            Reset();
        }

        /// <summary>
        /// オーバーフローフラグの値を取得します。
        /// </summary>
        internal Boolean OF
        {
            get { return m_overflowFlag; }
        }

        /// <summary>
        /// サインフラグの値を取得します。
        /// </summary>
        internal Boolean SF
        {
            get { return m_signFlag; }
        }

        /// <summary>
        /// ゼロフラグの値を取得します。
        /// </summary>
        internal Boolean ZF
        {
            get { return m_zeroFlag; }
        }

        /// <summary>
        /// フラグレジスタの値を初期化します。
        /// </summary>
        internal void Reset()
        {
            m_overflowFlag = false;
            m_signFlag = false;
            m_zeroFlag = false;
        }
    }
}
