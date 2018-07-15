using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Common
{
    /// <summary>
    /// COMET II の語を表わします。1 語は 16 ビットで、最下位がビット 0、最上位がビット 15 です。
    /// </summary>
    internal struct Word
    {
        #region Static Fields
        /// <summary>
        /// 値が 0 の語です。
        /// </summary>
        internal static readonly Word Zero = new Word(0);

        /// <summary>
        /// 値が 1 の語です。
        /// </summary>
        internal static readonly Word One = new Word(1);
        #endregion
        
        /// <summary>
        /// <see cref="UInt16"/> 型の値を <see cref="Word"/> に変換します。
        /// </summary>
        /// <param name="ui16Val"><see cref="Word"/> に変換する <see cref="UInt16"/> 型の値です。</param>
        public static implicit operator Word(UInt16 ui16Val)
        {
            return new Word(ui16Val);
        }

        /// <summary>
        /// <see cref="Word"/> の 2 つのオブジェクトが等しいかどうかを調べる '==' 演算子です。
        /// </summary>
        /// <param name="word1">比較する一方の <see cref="Word"/> のオブジェクトです。</param>
        /// <param name="word2">比較するもう一方の <see cref="Word"/> のオブジェクトです。</param>
        /// <returns>
        /// 比較する 2 つの <see cref="Word"/> オブジェクトの値が等しければ <see langword="true"/> を、
        /// 値が等しくなければ <see langword="false"/> を返します。
        /// </returns>
        public static Boolean operator==(Word word1, Word word2)
        {
            return word1.Equals(word2);
        }

        /// <summary>
        /// <see cref="Word"/> の 2 つのオブジェクトが等しくないかどうかを調べる '!=' 演算子です。
        /// </summary>
        /// <param name="word1">比較する一方の <see cref="Word"/> のオブジェクトです。</param>
        /// <param name="word2">比較するもう一方の <see cref="Word"/> のオブジェクトです。</param>
        /// <returns>
        /// 比較する 2 つの <see cref="Word"/> オブジェクトの値が等しくなければ <see langword="true"/> を、
        /// 値が等しれば <see langword="false"/> を返します。
        /// </returns>
        public static Boolean operator!=(Word word1, Word word2)
        {
            return !word1.Equals(word2);
        }

        #region Instance Fields
        // 1 語は 16 ビット。符号なしで格納します。
        private readonly UInt16 m_ui16Val;
        #endregion

        /// <summary>
        /// 指定の符号付き 16 ビットの値を用いて、<see cref="Word"/>のインスタンスを初期化します。
        /// </summary>
        /// <param name="i16Val">作成する語に格納する符号付き 16 ビットの値です。</param>
        internal Word(Int16 i16Val)
            : this(NumberUtils.ToUInt16(i16Val))
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
        /// 指定のオブジェクトが、このオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">このオブジェクトと比較するオブジェクトです。</param>
        /// <returns>
        /// 指定のオブジェクトがこのオブジェクトと等しい場合は <see langword="true"/> を、
        /// それ以外の場合は <see langword="false"/> を返します。
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            Word that = (Word)obj;
            return Equals(that);
        }

        internal Boolean Equals(Word that)
        {
            return this.m_ui16Val == that.m_ui16Val;
        }

        /// <summary>
        /// この語のハッシュコードを表わす値を取得します。
        /// </summary>
        /// <returns>この語のハッシュコードを表わす値を返します。</returns>
        public override Int32 GetHashCode()
        {
            return m_ui16Val;
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
            return NumberUtils.ToInt16(m_ui16Val);
        }

        /// <summary>
        /// 語に格納された値の指定のビット範囲の値を取得します。
        /// 最上位はビット 15、最下位はビット 0 です。
        /// </summary>
        /// <param name="fromUpperBit">取得する範囲の上位ビットを示す値です。</param>
        /// <param name="toLowerBit">取得する範囲の下位ビットを示す値です。</param>
        /// <returns>語に格納された値の指定のビット範囲の値を返します。</returns>
        internal UInt16 GetBits(Int32 fromUpperBit, Int32 toLowerBit)
        {
            return UInt16Utils.GetBits(m_ui16Val, fromUpperBit, toLowerBit);
        }

        /// <summary>
        /// 語に格納する値の符号が負かどうかを返します。
        /// 語に格納する値のビット 15 が 1 ならば、符号は負とします。
        /// </summary>
        /// <returns>
        /// 符号が負ならば <see langword="true"/>、それ以外ならば <see langword="false"/> を返します。
        /// </returns>
        internal Boolean IsMinus()
        {
            return (m_ui16Val & 0x8000) != 0;
        }

        /// <summary>
        /// 語に格納する値が零かどうかを返します。
        /// </summary>
        /// <returns>
        /// 値が零ならば <see langword="true"/>、それ以外ならば <see langword="false"/> を返します。
        /// </returns>
        internal Boolean IsZero()
        {
            return m_ui16Val == 0;
        }

        /// <summary>
        /// この語を表わす文字列を作成します。
        /// </summary>
        /// <returns>この語を表わす文字列を返します。</returns>
        public override String ToString()
        {
            String str = String.Format("{0} (0x{0:x04})", m_ui16Val);
            return str;
        }
    }
}
