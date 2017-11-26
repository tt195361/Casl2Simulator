using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Comet2
{
    /// <summary>
    /// 算術論理演算を行います (Arithmetic Logic Unit, ALU)。
    /// </summary>
    internal static class Alu
    {
        /// <summary>
        /// 演算メソッドを呼び出すデリゲートです。
        /// </summary>
        /// <param name="word1">演算する値を格納する第一の語です。</param>
        /// <param name="word2">演算する値を格納する第二の語です。</param>
        /// <param name="overflow">演算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>演算の結果を返します。</returns>
        internal delegate Word OperationMethod(Word word1, Word word2, out Boolean overflow);

        #region Arithmetic Operation
        private delegate Int32 ArithmeticOp(Int16 i16Val1, Int16 i16Val2);

        /// <summary>
        /// 指定の値を算術加算し、その結果とオーバーフローしたかどうかを返します。
        /// </summary>
        /// <param name="word1">加算する値を格納する第一の語です。</param>
        /// <param name="word2">加算する値を格納する第二の語です。</param>
        /// <param name="overflow">加算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>算術加算の結果を返します。</returns>
        internal static Word AddArithmetic(Word word1, Word word2, out Boolean overflow)
        {
            return ArithmeticOperation(
                (i16Val1, i16Val2) => i16Val1 + i16Val2, word1, word2, out overflow);
        }

        /// <summary>
        /// 指定の値を算術減算し、その結果とオーバーフローしたかどうかを返します。
        /// </summary>
        /// <param name="word1">減算される値を格納する語です。</param>
        /// <param name="word2">減算する値を格納する語です。</param>
        /// <param name="overflow">減算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>算術減算の結果を返します。</returns>
        internal static Word SubtractArithmetic(Word word1, Word word2, out Boolean overflow)
        {
            return ArithmeticOperation(
                (i16Val1, i16Val2) => i16Val1 - i16Val2, word1, word2, out overflow);
        }

        private static Word ArithmeticOperation(
            ArithmeticOp op, Word word1, Word word2, out Boolean overflow)
        {
            Int16 i16Val1 = word1.GetAsSigned();
            Int16 i16Val2 = word2.GetAsSigned();
            Int32 i32Val = op(i16Val1, i16Val2);

            Int16 i16Result = NumberUtils.ToInt16(i32Val);
            overflow = NumberUtils.CheckInt16Overflow(i32Val);
            return new Word(i16Result);
        }
        #endregion // Arithmetic Operation

        #region Logical Operation
        private delegate Int32 LogicalOp(UInt16 ui16Val1, UInt16 ui16Val2);

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
            return LogicalOperation(
                (ui16Val1, ui16Val2) => ui16Val1 + ui16Val2, word1, word2, out overflow);
        }

        /// <summary>
        /// 指定の値を論理減算し、その結果を返します。
        /// </summary>
        /// <param name="word1">減算される値を格納する語です。</param>
        /// <param name="word2">減算する値を格納する語です。</param>
        /// <returns>論理減算の結果を返します。</returns>
        internal static Word SubtractLogical(Word word1, Word word2)
        {
            Boolean notUsed;
            return SubtractLogical(word1, word2, out notUsed);
        }

        /// <summary>
        /// 指定の値を論理減算し、その結果とオーバーフローしたかどうかを返します。
        /// </summary>
        /// <param name="word1">減算される値を格納する語です。</param>
        /// <param name="word2">減算する値を格納する語です。</param>
        /// <param name="overflow">減算の結果がオーバーフローしたかどうかを返します。</param>
        /// <returns>論理減算の結果を返します。</returns>
        internal static Word SubtractLogical(Word word1, Word word2, out Boolean overflow)
        {
            return LogicalOperation(
                (ui16Val1, ui16Val2) => ui16Val1 - ui16Val2, word1, word2, out overflow);
        }

        /// <summary>
        /// 指定の値の論理積を求め、その結果を返します。
        /// オーバーフローは、常に <see langword="false"/> を返します。
        /// </summary>
        /// <param name="word1">論理積する値を格納する第一の語です。</param>
        /// <param name="word2">論理積する値を格納する第二の語です。</param>
        /// <param name="overflow">常に <see langword="false"/> を返します。</param>
        /// <returns>論理積の結果を返します。</returns>
        internal static Word And(Word word1, Word word2, out Boolean overflow)
        {
            return LogicalOperation(
                (ui16Val1, ui16Val2) => ui16Val1 & ui16Val2, word1, word2, out overflow);
        }

        /// <summary>
        /// 指定の値の論理和を求め、その結果を返します。
        /// オーバーフローは、常に <see langword="false"/> を返します。
        /// </summary>
        /// <param name="word1">論理和する値を格納する第一の語です。</param>
        /// <param name="word2">論理和する値を格納する第二の語です。</param>
        /// <param name="overflow">常に <see langword="false"/> を返します。</param>
        /// <returns>論理和の結果を返します。</returns>
        internal static Word Or(Word word1, Word word2, out Boolean overflow)
        {
            return LogicalOperation(
                (ui16Val1, ui16Val2) => ui16Val1 | ui16Val2, word1, word2, out overflow);
        }

        /// <summary>
        /// 指定の値の排他的論理和を求め、その結果を返します。
        /// オーバーフローは、常に <see langword="false"/> を返します。
        /// </summary>
        /// <param name="word1">排他的論理和する値を格納する第一の語です。</param>
        /// <param name="word2">排他的論理和する値を格納する第二の語です。</param>
        /// <param name="overflow">常に <see langword="false"/> を返します。</param>
        /// <returns>排他的論理和の結果を返します。</returns>
        internal static Word Xor(Word word1, Word word2, out Boolean overflow)
        {
            return LogicalOperation(
                (ui16Val1, ui16Val2) => ui16Val1 ^ ui16Val2, word1, word2, out overflow);
        }

        private static Word LogicalOperation(
            LogicalOp op, Word word1, Word word2, out Boolean overflow)
        {
            UInt16 ui16Val1 = word1.GetAsUnsigned();
            UInt16 ui16Val2 = word2.GetAsUnsigned();
            Int32 i32Val = op(ui16Val1, ui16Val2);

            UInt16 ui16Result = NumberUtils.ToUInt16(i32Val);
            overflow = NumberUtils.CheckUInt16Overflow(i32Val);
            return new Word(ui16Result);
        }
        #endregion // Logical Operation

        #region Compare
        /// <summary>
        /// 比較メソッドを呼び出すデリゲートです。
        /// </summary>
        /// <param name="word1">比較する値を格納する第一の語です。</param>
        /// <param name="word2">比較する値を格納する第二の語です。</param>
        /// <param name="sign">
        /// 比較の結果が負かどうかを返します。負になるのは <paramref name="word1"/> の値が
        /// <paramref name="word2"/> の値より小さい場合です。
        /// </param>
        /// <param name="zero">
        /// 比較の結果が零かどうかを返します。零になるのは <paramref name="word1"/> と
        /// <paramref name="word2"/> が同じ値の場合です。
        /// </param>
        internal delegate void CompareMethod(
            Word word1, Word word2, out Boolean sign, out Boolean zero);

        /// <summary>
        /// 指定の値を算術比較し、符号と零かどうかを返します。
        /// </summary>
        /// <param name="word1">比較する値を格納する第一の語です。</param>
        /// <param name="word2">比較する値を格納する第二の語です。</param>
        /// <param name="sign">
        /// 比較の結果が負かどうかを返します。負になるのは <paramref name="word1"/> の値が
        /// <paramref name="word2"/> の値より小さい場合です。
        /// </param>
        /// <param name="zero">
        /// 比較の結果が零かどうかを返します。零になるのは <paramref name="word1"/> と
        /// <paramref name="word2"/> が同じ値の場合です。
        /// </param>
        internal static void CompareArithmetic(Word word1, Word word2, out Boolean sign, out Boolean zero)
        {
            Int32 i32Val1 = word1.GetAsSigned();
            Int32 i32Val2 = word2.GetAsSigned();
            Compare(i32Val1, i32Val2, out sign, out zero);
        }

        /// <summary>
        /// 指定の値を論理比較し、符号と零かどうかを返します。
        /// </summary>
        /// <param name="word1">比較する値を格納する第一の語です。</param>
        /// <param name="word2">比較する値を格納する第二の語です。</param>
        /// <param name="sign">
        /// 比較の結果が負かどうかを返します。負になるのは <paramref name="word1"/> の値が
        /// <paramref name="word2"/> の値より小さい場合です。
        /// </param>
        /// <param name="zero">
        /// 比較の結果が零かどうかを返します。零になるのは <paramref name="word1"/> と
        /// <paramref name="word2"/> が同じ値の場合です。
        /// </param>
        internal static void CompareLogical(Word word1, Word word2, out Boolean sign, out Boolean zero)
        {
            Int32 i32Val1 = word1.GetAsUnsigned();
            Int32 i32Val2 = word2.GetAsUnsigned();
            Compare(i32Val1, i32Val2, out sign, out zero);
        }

        private static void Compare(Int32 i32Val1, Int32 i32Val2, out Boolean sign, out Boolean zero)
        {
            sign = (i32Val1 < i32Val2);
            zero = (i32Val1 == i32Val2);
        }
        #endregion // Compare

        #region Shift
        /// <summary>
        /// シフトメソッドを呼び出すデリゲートです。
        /// </summary>
        /// <param name="word1">シフトする値を格納する語です。</param>
        /// <param name="word2">シフトする回数を格納する語です。</param>
        /// <param name="lastShiftedOutBit">>最後に送り出されたビットの値を返します。</param>
        /// <returns>シフトした値を返します。</returns>
        internal delegate Word ShiftMethod(Word word1, Word word2, out UInt16 lastShiftedOutBit);

        /// <summary>
        /// 指定の値を指定の回数だけ左に算術シフトし、その結果と最後に送り出されたビットの値を返します。
        /// </summary>
        /// <param name="word1">シフトする値を格納する語です。</param>
        /// <param name="word2">シフトする回数を格納する語です。</param>
        /// <param name="lastShiftedOutBit">最後に送り出されたビットの値を返します。</param>
        /// <returns>指定の値を指定の回数だけ左に算術シフトした値を返します。</returns>
        internal static Word ShiftLeftArithmetic(Word word1, Word word2, out UInt16 lastShiftedOutBit)
        {
            Word shiftedWord = DoShift(
                ShiftArithmetic, ShiftLeft, UInt16Utils.MSB - 1, word1, word2, out lastShiftedOutBit);
            return shiftedWord;
        }

        /// <summary>
        /// 指定の値を指定の回数だけ左に論理シフトし、その結果と最後に送り出されたビットの値を返します。
        /// </summary>
        /// <param name="word1">シフトする値を格納する語です。</param>
        /// <param name="word2">シフトする回数を格納する語です。</param>
        /// <param name="lastShiftedOutBit">最後に送り出されたビットの値を返します。</param>
        /// <returns>指定の値を指定の回数だけ左に論理シフトした値を返します。</returns>
        internal static Word ShiftLeftLogical(Word word1, Word word2, out UInt16 lastShiftedOutBit)
        {
            Word shiftedWord = DoShift(
                ShiftLogical, ShiftLeft, UInt16Utils.MSB, word1, word2, out lastShiftedOutBit);
            return shiftedWord;
        }

        /// <summary>
        /// 指定の値を指定の回数だけ右に算術シフトし、その結果と最後に送り出されたビットの値を返します。
        /// </summary>
        /// <param name="word1">シフトする値を格納する語です。</param>
        /// <param name="word2">シフトする回数を格納する語です。</param>
        /// <param name="lastShiftedOutBit">最後に送り出されたビットの値を返します。</param>
        /// <returns>指定の値を指定の回数だけ右に算術シフトした値を返します。</returns>
        internal static Word ShiftRightArithmetic(Word word1, Word word2, out UInt16 lastShiftedOutBit)
        {
            Word shiftedWord = DoShift(
                ShiftArithmetic, ShiftRight, UInt16Utils.LSB, word1, word2, out lastShiftedOutBit);
            return shiftedWord;
        }

        /// <summary>
        /// 指定の値を指定の回数だけ右に論理シフトし、その結果と最後に送り出されたビットの値を返します。
        /// </summary>
        /// <param name="word1">シフトする値を格納する語です。</param>
        /// <param name="word2">シフトする回数を格納する語です。</param>
        /// <param name="lastShiftedOutBit">最後に送り出されたビットの値を返します。</param>
        /// <returns>指定の値を指定の回数だけ右に論理シフトした値を返します。</returns>
        internal static Word ShiftRightLogical(Word word1, Word word2, out UInt16 lastShiftedOutBit)
        {
            Word shiftedWord = DoShift(
                ShiftLogical, ShiftRight, UInt16Utils.LSB, word1, word2, out lastShiftedOutBit);
            return shiftedWord;
        }

        private delegate UInt16 ShiftTypeFunc(ShiftDirectionFunc shiftDirectionFunc, UInt16 ui16Val);
        private delegate UInt16 ShiftDirectionFunc(UInt16 ui16Val);

        private static Word DoShift(
            ShiftTypeFunc shiftTypeFunc, ShiftDirectionFunc shiftDirectionFunc, Int32 shiftedOutBitPos,
            Word word1, Word word2, out UInt16 lastShiftedOutBit)
        {
            UInt16 shiftedValue = word1.GetAsUnsigned();
            UInt16 shiftCount = word2.GetAsUnsigned();

            // BitSize (= 16) 回以上シフトしても、結果は変わらない。
            shiftCount = Math.Min((UInt16)UInt16Utils.BitSize, shiftCount);
            lastShiftedOutBit = 0;
            for (UInt16 shiftIndex = 0; shiftIndex < shiftCount; ++shiftIndex)
            {
                lastShiftedOutBit = UInt16Utils.GetBit(shiftedValue, shiftedOutBitPos);
                shiftedValue = shiftTypeFunc(shiftDirectionFunc, shiftedValue);
            }

            return new Word(shiftedValue);
        }

        private static UInt16 ShiftArithmetic(ShiftDirectionFunc shiftDirectionFunc, UInt16 ui16Val)
        {
            // 算術シフトでは、符号ビットの値はそのまま変化しない。
            UInt16 signBit = UInt16Utils.GetBit(ui16Val, UInt16Utils.MSB);
            UInt16 shiftedValue = ShiftLogical(shiftDirectionFunc, ui16Val);
            UInt16 shiftedSignedValue = UInt16Utils.SetBit(shiftedValue, UInt16Utils.MSB, signBit);
            return shiftedSignedValue;
        }

        private static UInt16 ShiftLogical(ShiftDirectionFunc shiftDirectionFunc, UInt16 ui16Val)
        {
            UInt16 shiftedValue = shiftDirectionFunc(ui16Val);
            return shiftedValue;
        }

        private static UInt16 ShiftLeft(UInt16 ui16Val)
        {
            return NumberUtils.ToUInt16(ui16Val << 1);
        }

        private static UInt16 ShiftRight(UInt16 ui16Val)
        {
            return NumberUtils.ToUInt16(ui16Val >> 1);
        }
        #endregion // Shift
    }
}
