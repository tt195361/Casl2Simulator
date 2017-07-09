using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// COMET II の語を表わします。1 語は 16 ビットで、最下位がビット 0、最上位がビット 15 です。
    /// </summary>
    internal struct Word
    {
        #region Fields
        // 値が 0 と 1 の語。
        internal static readonly Word Zero = new Word(0);
        internal static readonly Word One = new Word(1);

        // 最上位ビット (Most Significant Bit, MSB) と最下位ビット (Least Significant Bit, LSB) 
        private const Int32 MSB = 15;
        private const Int32 LSB = 0;

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
            ArgChecker.CheckRange(fromUpperBit, LSB, MSB, nameof(fromUpperBit));
            ArgChecker.CheckRange(toLowerBit, LSB, MSB, nameof(toLowerBit));
            ArgChecker.CheckGreaterEqual(fromUpperBit, toLowerBit, nameof(fromUpperBit), nameof(toLowerBit));

            UInt16 mask = MakeMask(fromUpperBit, toLowerBit);
            Int32 bits = ((m_ui16Val & mask) >> toLowerBit);
            return NumberUtils.ToUInt16(bits);
        }

        private UInt16 MakeMask(Int32 fromUpperBit, Int32 toLowerBit)
        {
            Int32 bitCount = fromUpperBit - toLowerBit + 1;
            Int32 maskBits = (1 << bitCount) - 1;
            Int32 shiftedMaskBits = maskBits << toLowerBit;
            return NumberUtils.ToUInt16(shiftedMaskBits);
        }

        /// <summary>
        /// 語に格納する値の符号が負かどうかを返します。
        /// 語に格納する値のビット 15 が 1 ならば、符号は負とします。
        /// </summary>
        /// <returns>符号が負ならば true、それ以外ならば false を返します。</returns>
        internal Boolean IsMinus()
        {
            return (m_ui16Val & 0x8000) != 0;
        }

        /// <summary>
        /// 語に格納する値が零かどうかを返します。
        /// </summary>
        /// <returns>値が零ならば true、それ以外ならば false を返します。</returns>
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
            return m_ui16Val.ToString();
        }
    }
}
