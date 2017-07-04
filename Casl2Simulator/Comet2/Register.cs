using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II のレジスタを表わします。
    /// </summary>
    internal class Register
    {
        #region Fields
        private Word m_value;
        #endregion

        /// <summary>
        /// <see cref="Register"/> のインスタンスを初期化します。
        /// </summary>
        internal Register()
        {
            Reset();
        }

        /// <summary>
        /// レジスタの値を取得または設定します。
        /// </summary>
        internal Word Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// レジスタの値を初期化します。
        /// </summary>
        internal void Reset()
        {
            Value = new Word(0);
        }

        /// <summary>
        /// このレジスタを表わす文字列を作成します。
        /// </summary>
        /// <returns>このレジスタを表わす文字列を返します。</returns>
        public override String ToString()
        {
            UInt16 ui16Val = m_value.GetAsUnsigned();
            return "0x" + ui16Val.ToString("x04");
        }
    }
}
