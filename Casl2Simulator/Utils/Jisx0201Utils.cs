using System;
using System.Linq;
using System.Text;
using Tt195361.Casl2Simulator.Properties;

namespace Tt195361.Casl2Simulator.Utils
{
    /// <summary>
    /// JIS X 0201 に関する汎用のメソッドを集めたクラスです。 
    /// </summary>
    internal static class Jisx0201Utils
    {
        #region Static Fields
        // csISO2022JP -- Japanese (JIS-Allow 1 byte Kana)
        private static readonly Encoding m_jisEncoding = Encoding.GetEncoding("csISO2022JP");

        private const Byte ESC = 0x1b;
        private const Byte OpenParen = 0x28;
        private const Byte I = 0x49;
        private const Byte B = 0x42;
        #endregion

        /// <summary>
        /// 指定の文字列中の各文字を表わす JIS X 0201 のコードを取得します。 
        /// </summary>
        /// <param name="str">JIS X 0201 の文字コードを取得する各文字を格納する文字列です。</param>
        /// <returns>
        /// JIS X 0201 のコードを格納する<see cref="Byte"/>の配列を返します。
        /// </returns>
        internal static Byte[] ToJisx0201Bytes(String str)
        {
            return str.Select((c) => ToJisx0201Byte(c))
                      .ToArray();
        }

        internal static Byte ToJisx0201Byte(Char c)
        {
            String s = c.ToString();
            Byte[] jisEncodingBytes = m_jisEncoding.GetBytes(s);
            return ToJisx0201Byte(jisEncodingBytes, c);
        }

        private static Byte ToJisx0201Byte(Byte[] jisEncodingBytes, Char c)
        {
            if (jisEncodingBytes.Length == 1)
            {
                return jisEncodingBytes[0];
            }

            if (jisEncodingBytes.Length == 7 &&
                jisEncodingBytes[0] == ESC && jisEncodingBytes[1] == OpenParen &&
                jisEncodingBytes[2] == I && jisEncodingBytes[4] == ESC &&
                jisEncodingBytes[5] == OpenParen && jisEncodingBytes[6] == B)
            {
                // 最上位ビットが 1 の場合、前にシフトインが 3 バイト、後にシフトアウトが 3 バイト付く。
                return (Byte)(jisEncodingBytes[3] | 0x80);
            }

            String message = String.Format(Resources.MSG_CanNotRepresentInJisx0201, c);
            throw new Casl2SimulatorException(message);
        }
    }
}
