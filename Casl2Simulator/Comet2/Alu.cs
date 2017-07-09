using System;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 算術論理演算を行います (Arithmetic Logic Unit, ALU)。
    /// </summary>
    internal static class Alu
    {
        /// <summary>
        /// 指定の値を算術加算し、その結果とオーバーフローしたかどうかを返します。
        /// </summary>
        /// <param name="word1">加算する値を格納する第一の語です。</param>
        /// <param name="word2">加算する値を格納する第二の語です。</param>
        /// <param name="overflow">加算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>算術加算の結果を返します。</returns>
        internal static Word AddArithmetic(Word word1, Word word2, out Boolean overflow)
        {
            Int16 i16Val1 = word1.GetAsSigned();
            Int16 i16Val2 = word2.GetAsSigned();
            Int32 i32Val = i16Val1 + i16Val2;

            Int16 i16Result = NumberUtils.ToInt16(i32Val);
            overflow = NumberUtils.CheckInt16Overflow(i32Val);
            return new Word(i16Result);
        }

        /// <summary>
        /// 指定の値を論理加算し、その結果を返します。
        /// </summary>
        /// <param name="word1">加算する値を格納する第一の語です。</param>
        /// <param name="word2">加算する値を格納する第二の語です。</param>
        /// <returns>論理加算の結果を返します。</returns>
        internal static Word AddLogical(Word word1, Word word2)
        {
            Boolean notUsed;
            return AddLogical(word1, word2, out notUsed);
        }

        /// <summary>
        /// 指定の値を論理加算し、その結果とオーバーフローしたかどうかを返します。
        /// </summary>
        /// <param name="word1">加算する値を格納する第一の語です。</param>
        /// <param name="word2">加算する値を格納する第二の語です。</param>
        /// <param name="overflow">加算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>論理加算の結果を返します。</returns>
        internal static Word AddLogical(Word word1, Word word2, out Boolean overflow)
        {
            UInt16 ui16Val1 = word1.GetAsUnsigned();
            UInt16 ui16Val2 = word2.GetAsUnsigned();
            Int32 i32Val = ui16Val1 + ui16Val2;

            UInt16 ui16Result = NumberUtils.ToUInt16(i32Val);
            overflow = NumberUtils.CheckUInt16Overflow(i32Val);
            return new Word(ui16Result);
        }
    }
}
