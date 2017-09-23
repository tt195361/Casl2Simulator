using System;
using System.Text;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 文字定数です。
    /// </summary>
    internal class StringConstant : Constant
    {
        /// <summary>
        /// 指定の文字が文字定数の最初の文字かどうかを判断します。
        /// </summary>
        /// <param name="firstChar">文字定数の最初かどうかを判断する対象の文字です。</param>
        /// <returns>
        /// 指定の文字が文字定数の最初の文字なら <see langword="true"/> を、
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsStart(Char firstChar)
        {
            return firstChar == Casl2Defs.SingleQuote;
        }

        /// <summary>
        /// 文字定数を解釈します。
        /// </summary>
        /// <param name="buffer">解釈する文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>
        /// 解釈した結果として生成した <see cref="StringConstant"/> クラスのオブジェクトを返します。
        /// </returns>
        internal static StringConstant Parse(ReadBuffer buffer)
        {
            String value = GetString(buffer);
            return new StringConstant(value);
        }

        private static String GetString(ReadBuffer buffer)
        {
            Int32 parseStartIndex = buffer.CurrentIndex;
            buffer.SkipExpected(Casl2Defs.SingleQuote);

            StringBuilder builder = new StringBuilder();

            for ( ; ; )
            {
                if (buffer.Current == ReadBuffer.EndOfStr)
                {
                    String str = buffer.GetToCurrent(parseStartIndex);
                    String message = String.Format(Resources.MSG_NoCloseQuoteInStrConstant, str);
                    throw new Casl2SimulatorException(message);
                }

                Char? c = GetChar(buffer);
                if (c == null)
                {
                    break;
                }

                builder.Append(c);
            }

            return builder.ToString();
        }

        private static Char? GetChar(ReadBuffer buffer)
        {
            Char? result;

            if (buffer.Current != Casl2Defs.SingleQuote)
            {
                // 文字定数では、空白やコンマも文字定数に含める。
                result = buffer.Current;
                buffer.MoveNext();
            }
            else
            {
                buffer.MoveNext();
                if (buffer.Current != Casl2Defs.SingleQuote)
                {
                    // 閉じ側のシングルクォート
                    result = null;
                }
                else
                {
                    // アポストロフィは 2 個続けて書く。
                    result = Casl2Defs.SingleQuote;
                    buffer.MoveNext();
                }
            }

            return result;
        }

        #region Fields
        private readonly String m_value;
        #endregion

        private StringConstant(String value)
        {
            m_value = value;
        }

        internal String Value
        {
            get { return m_value; }
        }

        internal override int GetWordCount()
        {
            return m_value.Length;
        }

        internal override void GenerateCode(LabelManager lblManager, RelocatableModule relModule)
        {
            throw new NotImplementedException();
        }
    }
}
