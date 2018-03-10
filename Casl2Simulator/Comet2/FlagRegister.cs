using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II のフラグレジスタ (Flag Register) を表わします。
    /// </summary>
    internal class FlagRegister
    {
        #region Instance Fields
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
            SetFlags(false, false, false);
        }

        /// <summary>
        /// それぞれのフラグの値を設定します。オーバーフローフラグは指定の値を、
        /// サインフラグとゼロフラグは指定のレジスタの値から設定します。
        /// </summary>
        /// <param name="r">サインフラグとゼロフラグの値の設定に使用するレジスタです。</param>
        /// <param name="overflowFlag">オーバーフローフラグに設定する値です。</param>
        internal void SetFlags(Register r, Boolean overflowFlag)
        {
            Boolean signFlag = r.Value.IsMinus();
            Boolean zeroFlag = r.Value.IsZero();
            SetFlags(overflowFlag, signFlag, zeroFlag);
        }

        /// <summary>
        /// それぞれのフラグを指定の値に設定します。
        /// </summary>
        /// <param name="overflowFlag">オーバーフローフラグに設定する値です。</param>
        /// <param name="signFlag">サインフラグに設定する値です。</param>
        /// <param name="zeroFlag">ゼロフラグに設定する値です。</param>
        internal void SetFlags(Boolean overflowFlag, Boolean signFlag, Boolean zeroFlag)
        {
            m_overflowFlag = overflowFlag;
            m_signFlag = signFlag;
            m_zeroFlag = zeroFlag;
        }
    }
}
