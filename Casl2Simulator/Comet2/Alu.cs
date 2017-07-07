using System;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 算術論理演算を行います (Arithmetic Logic Unit, ALU)。
    /// </summary>
    internal static class Alu
    {
        /// <summary>
        /// 指定の値を算術加算します。
        /// </summary>
        /// <param name="word1">加算する値を格納する第一の語です。</param>
        /// <param name="word2">加算する値を格納する第二の語です。</param>
        /// <param name="overflow">加算の結果がオーバーフローしたかどうかを設定します。</param>
        /// <returns>算術加算の結果を返します。</returns>
        internal static Word AddArithmetic(Word word1, Word word2, out Boolean overflow)
        {
            Int16 i16Val1 = word1.GetAsSigned();
            Int16 i16Val2 = word2.GetAsSigned();
            Int32 i32Val = i16Val1 + i16Val2;

            Int16 i16Result = ToInt16(i32Val);
            overflow = CheckInt16Overflow(i32Val);
            return new Word(i16Result);
        }

        private static Int16 ToInt16(Int32 i32Val)
        {
            // オーバーフローチェックなし。
            unchecked
            {
                Int16 i16Val = (Int16)i32Val;
                return i16Val;
            }
        }

        private static Boolean CheckInt16Overflow(Int32 i32Val)
        {
            // オーバーフローチェックあり。
            checked
            {
                try
                {
                    Int16 notUsed = (Int16)i32Val;
                    return false;
                }
                catch (OverflowException)
                {
                    return true;
                }
            }
        }
    }
}
