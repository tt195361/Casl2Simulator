using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の語を表わします。1 語は 16 ビットで、最下位がビット 0、最上位がビット 15 です。
    /// </summary>
    internal struct Word
    {
        #region Fields
        // 1 語は 16 ビット。符号なしで格納します。
        private readonly UInt16 m_ui16Val;
        #endregion

        /// <summary>
        /// 指定の符号付き 16 ビットの値を用いて、<see cref="Word"/>のインスタンスを初期化します。
        /// </summary>
        /// <param name="i16Val">作成する語に格納する符号付き 16 ビットの値です。</param>
        internal Word(Int16 i16Val)
            : this((UInt16)i16Val)
        {
            //
        }

        /// <summary>
        /// 指定の符号なし 16 ビットの値を用いて、<see cref="Word"/>のインスタンスを初期化します。
        /// </summary>
        /// <param name="ui16Val">作成する語に格納する符号なし 16 ビットの値です。</param>
        internal Word(UInt16 ui16Val)
        {
            m_ui16Val = ui16Val;
        }

        /// <summary>
        /// 語に格納された値を、符号なし 16 ビットとして取得します。
        /// </summary>
        /// <returns>符号なし 16 ビットの値を返します。</returns>
        internal UInt16 GetAsUnsigned()
        {
            return m_ui16Val;
        }

        /// <summary>
        /// 語に格納された値を、符号あり 16 ビットとして取得します。
        /// </summary>
        /// <returns>符号あり 16 ビットの値を返します。</returns>
        internal Int16 GetAsSigned()
        {
            return (Int16)m_ui16Val;
        }

        /// <summary>
        /// この語を表す文字列を作成します。
        /// </summary>
        /// <returns>この語を表す文字列を返します。</returns>
        public override String ToString()
        {
            return m_ui16Val.ToString();
        }
    }
}
