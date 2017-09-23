using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// 文字に関するメソッドを集めました。
    /// </summary>
    internal static class CharUtils
    {
        #region Static Fields
        private static readonly Encoding ShiftJis = Encoding.GetEncoding("Shift_Jis");

        // https://stackoverflow.com/questions/3253247/how-do-i-detect-non-printable-characters-in-net
        private static readonly UnicodeCategory[] NonRenderingCatgories = new UnicodeCategory[]
        {
            UnicodeCategory.Control,
            UnicodeCategory.OtherNotAssigned,
            UnicodeCategory.Surrogate,
        };
        #endregion

        /// <summary>
        /// 半角の大文字かどうか判断します。
        /// </summary>
        /// <param name="c">判断する文字です。</param>
        /// <returns>
        /// 半角の大文字の場合 <see langword="true"/> を返します。
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsHankakuUpper(Char c)
        {
            return Char.IsUpper(c) && IsHankaku(c);
        }

        /// <summary>
        /// 半角の数字かどうか判断します。
        /// </summary>
        /// <param name="c">判断する文字です。</param>
        /// <returns>
        /// 半角の数字の場合 <see langword="true"/> を返します。
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsHankakuDigit(Char c)
        {
            return Char.IsDigit(c) && IsHankaku(c);
        }

        /// <summary>
        /// 半角の 16 進数の文字かどうか判断します。
        /// </summary>
        /// <param name="c">判断する文字です。</param>
        /// <returns>
        /// 半角の 16 進数の文字の場合 <see langword="true"/> を返します。
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        internal static Boolean IsHankakuHexDigit(Char c)
        {
            return (Char.IsDigit(c) || ('A' <= c && c <= 'F')) && IsHankaku(c);
        }

        /// <summary>
        /// 半角の文字かどうか判断します。
        /// </summary>
        /// <param name="c">判断する文字です。</param>
        /// <returns>
        /// 半角の文字の場合 <see langword="true"/> を返します。
        /// それ以外は <see langword="false"/> を返します。
        /// </returns>
        private static Boolean IsHankaku(Char c)
        {
            // シフト JIS でエンコードして 1 バイトならば、半角。
            String str = c.ToString();
            Int32 byteCount = ShiftJis.GetByteCount(str);
            return byteCount == 1;
        }

        /// <summary>
        /// 指定の文字を指定の基数を用いて数字に変換します。
        /// </summary>
        /// <param name="c">数字に変換する文字です。</param>
        /// <param name="fromBase">変換に使用する基数です。</param>
        /// <returns>変換した数字を返します。</returns>
        internal static Int32 ToDigit(Char c, Int32 fromBase)
        {
            return Convert.ToInt32(c.ToString(), fromBase);
        }

        /// <summary>
        /// 指定の文字の印字可能な表現に変換します。
        /// </summary>
        /// <param name="c">印字可能な表現に変換する文字です。</param>
        /// <returns>指定の文字の印字可能な表現の文字列を返します。</returns>
        internal static String ToPrintable(Char c)
        {
            if (IsPrintable(c))
            {
                return c.ToString();
            }
            else
            {
                Int32 code = c;
                return String.Format(@"\u{0:X04}", code);
            }
        }

        private static Boolean IsPrintable(Char c)
        {
            UnicodeCategory uc = Char.GetUnicodeCategory(c);
            return !NonRenderingCatgories.Contains(uc);
        }
    }
}
