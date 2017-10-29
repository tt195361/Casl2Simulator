using System;
using System.Collections.Generic;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

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
        /// 文字定数を読み込みます。
        /// </summary>
        /// <param name="buffer">読み込む文字列が入った <see cref="ReadBuffer"/> のオブジェクトです。</param>
        /// <returns>読み込んだ文字列を返します。</returns>
        internal static String Read(ReadBuffer buffer)
        {
            return ReadChars(buffer).ConcatChars();
        }

        private static IEnumerable<Char> ReadChars(ReadBuffer buffer)
        {
            Int32 parseStartIndex = buffer.CurrentIndex;
            buffer.SkipExpected(Casl2Defs.SingleQuote);

            for ( ; ; )
            {
                if (buffer.Current == ReadBuffer.EndOfStr)
                {
                    String str = buffer.GetToCurrent(parseStartIndex);
                    String message = String.Format(Resources.MSG_NoCloseQuoteInStrConstant, str);
                    throw new Casl2SimulatorException(message);
                }

                Char? c = ReadChar(buffer);
                if (c == null)
                {
                    break;
                }

                yield return c.Value;
            }
        }

        private static Char? ReadChar(ReadBuffer buffer)
        {
            Char? result;

            if (buffer.Current != Casl2Defs.SingleQuote)
            {
                // 文字定数を囲むシングルクォートの内側。その文字が結果で、次の文字に進む。
                result = buffer.Current;
                buffer.MoveNext();
            }
            else
            {
                // シングルクォート、シングルクォートが 2 個続いているかどうか調べる。
                buffer.MoveNext();
                if (buffer.Current != Casl2Defs.SingleQuote)
                {
                    // シングルクォートが 2 個続いていなければ、閉じ側のシングルクォート。
                    result = null;
                }
                else
                {
                    // シングルクォートが 2 個続いていれば、シングルクォート 1 つで、次の文字に進む。
                    result = Casl2Defs.SingleQuote;
                    buffer.MoveNext();
                }
            }

            return result;
        }

        #region Fields
        private readonly String m_value;
        #endregion

        internal StringConstant(String value)
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

        protected override String ValueToString()
        {
            return ValueToString(m_value);
        }

        internal static String ValueToString(String value)
        {
            return WriteChars(value).ConcatChars();
        }

        private static IEnumerable<Char> WriteChars(String value)
        {
            yield return Casl2Defs.SingleQuote;

            foreach (Char c in value)
            {
                if (c != Casl2Defs.SingleQuote)
                {
                    yield return c;
                }
                else
                {
                    // アポストロフィは 2 個続けて書く。
                    yield return Casl2Defs.SingleQuote;
                    yield return Casl2Defs.SingleQuote;
                }
            }

            yield return Casl2Defs.SingleQuote;
        }
    }
}
