using System;
using Tt195361.Casl2Simulator.Properties;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2Simulator.Casl2
{
    /// <summary>
    /// 文字列を保持し読み取り位置を管理します。
    /// </summary>
    internal class ReadBuffer
    {
        #region Fields
        internal const Char EndOfStr = '\xffff';

        private readonly String m_str;
        private Int32 m_currentIndex;
        #endregion

        internal ReadBuffer(String str)
        {
            m_str = str;
            m_currentIndex = 0;
        }

        /// <summary>
        /// 現在位置の文字を取得します。
        /// 現在位置が文字列の終わりならば、文字として <see cref="EndOfStr"/> を返します。
        /// </summary>
        internal Char Current
        {
            get
            {
                if (HasEndOfStr())
                {
                    return EndOfStr;
                }
                else
                {
                    return m_str[m_currentIndex];
                }
            }
        }

        /// <summary>
        /// 現在位置のインデックスを取得します。
        /// 最初の文字のインデックスは 0、最後の文字のインデックスは Length - 1 です。
        /// 現在位置が最後の文字を越えると、Length を返します。
        /// </summary>
        internal Int32 CurrentIndex
        {
            get { return m_currentIndex; }
        }

        private Boolean HasEndOfStr()
        {
            return m_str.Length <= m_currentIndex;
        }

        private String GetPrintableCurrent()
        {
            if (Current == EndOfStr)
            {
                return Resources.STR_EndOfStr;
            }
            else
            {
                return CharUtils.ToPrintable(Current);
            }
        }

        /// <summary>
        /// 現在位置を次の文字に移動します。
        /// </summary>
        internal void MoveNext()
        {
            if (!HasEndOfStr())
            {
                ++m_currentIndex;
            }
        }

        /// <summary>
        /// 現在位置を次の空白でない文字に移動します。
        /// </summary>
        internal void SkipSpace()
        {
            SkipWhile(Char.IsWhiteSpace);
        }

        /// <summary>
        /// 現在位置を文字列の最後に移動します。
        /// </summary>
        internal void SkipToEnd()
        {
            SkipWhile((c) => true);
        }

        private void SkipWhile(Func<Char, Boolean> condition)
        {
            while (!HasEndOfStr() && condition(Current))
            {
                MoveNext();
            }
        }

        /// <summary>
        /// 現在位置が指定の文字であれば、現在位置を次の文字に移動します。
        /// </summary>
        /// <param name="expected">現在位置の文字かどうかをチェックする文字です。</param>
        internal void SkipExpected(Char expected)
        {
            if (Current != expected)
            {
                String printableCurrent = GetPrintableCurrent();
                String message = String.Format(Resources.MSG_NotExpectedChar, expected, printableCurrent);
                throw new Casl2SimulatorException(message);
            }

            MoveNext();
        }

        /// <summary>
        /// 現在位置から指定の<paramref name="condition"/>が<see langword="true"/>を返す間、文字列を読み込みます。
        /// 現在位置は読み込んだ文字列の次の位置に移動します。
        /// </summary>
        /// <param name="condition">読み込むかどうかを判断する関数です。</param>
        /// <returns></returns>
        internal String ReadWhile(Func<Char, Boolean> condition)
        {
            Int32 fromIndex = CurrentIndex;

            while (!HasEndOfStr() && condition(Current))
            {
                MoveNext();
            }

            return GetToCurrent(fromIndex);
        }

        /// <summary>
        /// 現在位置から最後までの文字列を取得します。
        /// 現在位置はそのまま移動しません。
        /// </summary>
        /// <returns>現在位置から最後までの文字列を返します。</returns>
        internal String GetRest()
        {
            return m_str.Substring(m_currentIndex);
        }

        /// <summary>
        /// 指定の位置から現在位置までの文字列を取得します。
        /// 現在位置はそのまま移動しません。
        /// </summary>
        /// <param name="fromIndex">取得する文字列の開始位置を指定します。</param>
        /// <returns>指定の位置から現在位置までの文字列を返します。</returns>
        internal String GetToCurrent(Int32 fromIndex)
        {
            Int32 length = m_currentIndex - fromIndex;
            return m_str.Substring(fromIndex, length);
        }

        /// <summary>
        /// このオブジェクトを表わす文字列を作成します。
        /// </summary>
        /// <returns>このオブジェクトを表わす文字列を返します。</returns>
        public override String ToString()
        {
            // デバッグ時に現在位置がわかるように、現在位置から残りを表示する。
            return GetRest();
        }
    }
}
