using System;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// <see cref="UInt16"/> 型の操作で共通に使うメソッドを集めました。
    /// </summary>
    internal static class UInt16Utils
    {
        /// <summary>
        /// <see cref="UInt16"/> 型の最上位ビット (Most Significant Bit, MSB) の値です。
        /// </summary>
        internal const Int32 MSB = 15;

        /// <summary>
        /// <see cref="UInt16"/> 型の最下位ビット (Least Significant Bit, LSB) の値です。
        /// </summary>
        internal const Int32 LSB = 0;

        /// <summary>
        /// <see cref="UInt16"/> 型のビットでのサイズを示す値です。
        /// </summary>
        internal const Int32 BitSize = 16;

        /// <summary>
        /// 指定の値の指定のビットの値を取得します。
        /// </summary>
        /// <param name="ui16Val">指定のビットの値を取得する値です。</param>
        /// <param name="bit">取得するビットを示す値です。</param>
        /// <returns>指定の値の指定のビットの値を返します。</returns>
        internal static UInt16 GetBit(UInt16 ui16Val, Int32 bit)
        {
            return GetBits(ui16Val, bit, bit);
        }

        /// <summary>
        /// 指定の値の指定のビット範囲の値を取得します。
        /// 最上位はビット 15、最下位はビット 0 です。
        /// </summary>
        /// <param name="ui16Val">指定のビット範囲の値を取得する値です。</param>
        /// <param name="fromUpperBit">取得する範囲の上位ビットを示す値です。</param>
        /// <param name="toLowerBit">取得する範囲の下位ビットを示す値です。</param>
        /// <returns>語に格納された値の指定のビット範囲の値を返します。</returns>
        internal static UInt16 GetBits(UInt16 ui16Val, Int32 fromUpperBit, Int32 toLowerBit)
        {
            CheckBitRange(fromUpperBit, toLowerBit);

            Int32 mask = MakeMask(fromUpperBit, toLowerBit);
            UInt16 bits = NumberUtils.ToUInt16((ui16Val & mask) >> toLowerBit);
            return bits;
        }

        /// <summary>
        /// 指定の値の指定のビットの値を設定します。
        /// </summary>
        /// <param name="ui16Val">指定のビットの値を設定する値です。</param>
        /// <param name="bit">設定するビットを示す値です。</param>
        /// <param name="ui16ValToSet">指定のビットに設定する値です。</param>
        /// <returns>指定の値の指定のビットに指定の値を設定し返します。</returns>
        internal static UInt16 SetBit(UInt16 ui16Val, Int32 bit, UInt16 ui16ValToSet)
        {
            return SetBits(ui16Val, bit, bit, ui16ValToSet);
        }

        /// <summary>
        /// 指定の値の指定のビット範囲の値を設定します。
        /// 最上位はビット 15、最下位はビット 0 です。
        /// </summary>
        /// <param name="ui16Val">指定のビット範囲の値を設定する値です。</param>
        /// <param name="fromUpperBit">設定する範囲の上位ビットを示す値です。</param>
        /// <param name="toLowerBit">設定する範囲の下位ビットを示す値です。</param>
        /// <param name="ui16ValToSet">指定のビット範囲に設定する値です。</param>
        /// <returns>語に格納された値の指定のビット範囲に指定の値を設定し返します。</returns>
        internal static UInt16 SetBits(
            UInt16 ui16Val, Int32 fromUpperBit, Int32 toLowerBit, UInt16 ui16ValToSet)
        {
            CheckBitRange(fromUpperBit, toLowerBit);

            Int32 mask = MakeMask(fromUpperBit, toLowerBit);
            Int32 maskedOutValue = ui16Val & ~mask;
            Int32 shiftedValueToSet = (ui16ValToSet << toLowerBit) & mask;
            Int32 i32Result = maskedOutValue | shiftedValueToSet;
            return NumberUtils.ToUInt16(i32Result);
        }

        private static void CheckBitRange(Int32 fromUpperBit, Int32 toLowerBit)
        {
            ArgChecker.CheckRange(fromUpperBit, LSB, MSB, nameof(fromUpperBit));
            ArgChecker.CheckRange(toLowerBit, LSB, MSB, nameof(toLowerBit));
            ArgChecker.CheckGreaterEqual(fromUpperBit, toLowerBit, nameof(fromUpperBit), nameof(toLowerBit));
        }

        private static Int32 MakeMask(Int32 fromUpperBit, Int32 toLowerBit)
        {
            Int32 bitCount = fromUpperBit - toLowerBit + 1;
            Int32 maskBits = (1 << bitCount) - 1;
            Int32 shiftedMaskBits = maskBits << toLowerBit;
            return shiftedMaskBits;
        }
    }
}
