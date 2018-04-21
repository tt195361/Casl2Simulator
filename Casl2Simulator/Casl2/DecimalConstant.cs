using System;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 10 進の数値定数です。
    /// </summary>
    internal class DecimalConstant : Constant, IAdrCodeGenerator
    {
        /// <summary>
        /// 指定の文字が 10 進定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">10 進定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字が 10 進定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsStart(Char firstChar)
        {
            return firstChar == Casl2Defs.Minus || CharUtils.IsHankakuDigit(firstChar);
        }

        /// <summary>
        /// 10 進定数を読み込みます。
        /// </summary>
        /// <param name="buffer">読み込む文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>読み込んだ <see cref="Int32"/> の値を返します。</returns>
        internal static Int32 Read(ReadBuffer buffer)
        {
            Int32 sign = GetSign(buffer);
            Int32 absValue = GetAbsValue(buffer);
            return sign * absValue;
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

        private static Int32 GetAbsValue(ReadBuffer buffer)
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

        #region Instance Fields
        private const Int32 MinValue = Int16.MinValue;
        private const Int32 MaxValue = Int16.MaxValue;

        private readonly Int16 m_value;
        #endregion

        /// <summary>
        /// 指定の値で 10 進定数を初期化します。10 進定数は -32768 ~ 32767 の範囲の数値です。
        /// </summary>
        internal DecimalConstant(Int32 value)
        {
            // 仕様書には、"-32768 ~ 32767 の範囲にないときは、その下位 16 ビットを格納する" と書かれています。
            // この通り実装し、プログラムで値を変更すると、使っている人は、なぜそうなるか理解できないと思います。
            // そのため、範囲外のときは例外にして、範囲内の値に書き直してもらうようにします。
            ArgChecker.CheckRange(value, MinValue, MaxValue, Resources.STR_DecimalConstant);

            m_value = NumberUtils.ToInt16(value);
        }

        internal Int16 Value
        {
            get { return m_value; }
        }

        public override Int32 GetCodeWordCount()
        {
            return 1;
        }

        public override void GenerateCode(RelocatableModule relModule)
        {
            Word word = new Word(m_value);
            relModule.AddWord(word);
        }

        public String GenerateLiteralDc(LabelTable lblTable)
        {
            return null;
        }

        protected override String ValueToString()
        {
            return ValueToString(m_value);
        }

        internal static String ValueToString(Int32 value)
        {
            return value.ToString();
        }
    }
}
