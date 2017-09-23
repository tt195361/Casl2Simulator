using System;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 10 進と 16 進の数値定数です。
    /// </summary>
    internal class NumericConstant : Constant
    {
        #region Fields
        private const Int32 MinValue = Int16.MinValue;
        private const Int32 MaxValue = Int16.MaxValue;
        #endregion

        /// <summary>
        /// 指定の文字が 10 進定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">10 進定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字が 10 進定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsDecimalStart(Char firstChar)
        {
            return firstChar == Casl2Defs.Minus || CharUtils.IsHankakuDigit(firstChar);
        }

        /// <summary>
        /// 10 進定数を解釈します。10 進定数は -32768 ~ 32767 の範囲の数値です。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="NumericConstant"/> クラスのオブジェクトを返します。
        /// </returns>
        /// <remarks>
        /// 仕様書には、"-32768 ~ 32767 の範囲にないときは、その下位 16 ビットを格納する" と書かれています。
        /// この通り実装し、プログラムで値を変更すると、使っている人は、なぜそうなるか理解できないと思います。
        /// そのため、範囲外のときは例外にして、範囲内の値に書き直してもらうようにします。
        /// </remarks>
        internal static NumericConstant ParseDecimal(ReadBuffer buffer)
        {
            Int32 sign = GetSign(buffer);
            Int32 absValue = GetDecimalAbsValue(buffer);
            Int32 signedValue = sign * absValue;
            if (signedValue < MinValue || MaxValue < signedValue)
            {
                String message = String.Format(
                    Resources.MSG_DecimalConstantOutOfRange, signedValue, MinValue, MaxValue);
                throw new Casl2SimulatorException(message);
            }

            return new NumericConstant(signedValue);
        }

        private static Int32 GetSign(ReadBuffer buffer)
        {
            if (buffer.Current != Casl2Defs.Minus)
            {
                return 1;
            }
            else
            {
                buffer.MoveNext();
                return -1;
            }
        }

        private static Int32 GetDecimalAbsValue(ReadBuffer buffer)
        {
            const Int32 Base = 10;

            Char c = buffer.Current;
            if (!CharUtils.IsHankakuDigit(c))
            {
                String message = String.Format(Resources.MSG_InvalidCharForDecimalConstant, c);
                throw new Casl2SimulatorException(message);
            }

            Int32 absValue = CharUtils.ToDigit(c, Base);
            buffer.MoveNext();

            for ( ; ; )
            {
                c = buffer.Current;
                if (!CharUtils.IsHankakuDigit(c))
                {
                    return absValue;
                }

                Int32 digit = CharUtils.ToDigit(c, Base);
                absValue = absValue * Base + digit;
                buffer.MoveNext();
            }
        }

        /// <summary>
        /// 指定の文字が 16 進定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">16 進定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字が 16 進定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsHexaDecimalStart(Char firstChar)
        {
            return firstChar == Casl2Defs.Sharp;
        }

        /// <summary>
        /// 16 進定数を解釈します。16 進定数は #h の形式で記述し、h は 4 桁の 16 進数
        /// (16 新数字は 0 ~ 9, A ~ F) です。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="NumericConstant"/> クラスのオブジェクトを返します。
        /// </returns>
        internal static NumericConstant ParseHexaDecimal(ReadBuffer buffer)
        {
            Int32 parseStartIndex = buffer.CurrentIndex;
            buffer.SkipExpected(Casl2Defs.Sharp);

            Int32 digitCount = 0;
            Int32 hexValue = GetHexValue(buffer, out digitCount);
            if (digitCount != 4)
            {
                String hexStr = buffer.GetToCurrent(parseStartIndex);
                String message = String.Format(Resources.MSG_InvalidHexConstantDigitCount, hexStr, digitCount);
                throw new Casl2SimulatorException(message);
            }

            return new NumericConstant(hexValue);
        }

        private static Int32 GetHexValue(ReadBuffer buffer, out Int32 digitCount)
        {
            const Int32 Base = 16;

            Int32 hexValue = 0;
            digitCount = 0;
            for ( ; ; )
            {
                Char c = buffer.Current;
                if (!CharUtils.IsHankakuHexDigit(c))
                {
                    return hexValue;
                }

                Int32 digit = CharUtils.ToDigit(c, Base);
                hexValue = hexValue * Base + digit;
                buffer.MoveNext();
                ++digitCount;
            }
        }

        #region Fields
        private readonly Int32 m_value;
        #endregion

        private NumericConstant(Int32 value)
        {
            m_value = value;
        }

        internal Int32 Value
        {
            get { return m_value; }
        }

        internal override int GetWordCount()
        {
            return 1;
        }

        internal override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            // TODO: 実装する。
            throw new NotImplementedException();
        }
    }
}
