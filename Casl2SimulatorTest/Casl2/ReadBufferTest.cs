using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// ReadBuffer クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ReadBufferTest
    {
        #region Fields
        private ReadBuffer m_target;
        #endregion

        /// <summary>
        /// 空行の場合のテストです。
        /// </summary>
        [TestMethod]
        public void Current_EmptyLine()
        {
            m_target = new ReadBuffer(String.Empty);
            CheckCurrent(ReadBuffer.EndOfStr, "最初から EndOfLine");
        }

        /// <summary>
        /// 中身がある場合のテストです。
        /// </summary>
        [TestMethod]
        public void Current_HasContentsLine()
        {
            m_target = new ReadBuffer("123");

            CheckCurrent('1', "作成時は 1 文字目");

            m_target.MoveNext();
            CheckCurrent('2', "MoveNext 1 回目: 2 文字目");

            m_target.MoveNext();
            CheckCurrent('3', "MoveNext 2 回目: 3 文字目");

            m_target.MoveNext();
            CheckCurrent(ReadBuffer.EndOfStr, "MoveNext 3 回目: 行末を越えると EndOfLine");
        }

        /// <summary>
        /// SkipSpace メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void SkipSpace()
        {
            CheckSkipSpace(String.Empty, ReadBuffer.EndOfStr, "空文字列 => 移動せず EndOfLine");
            CheckSkipSpace("あ", 'あ', "現在位置が空白でない => 現在位置は移動せず");
            CheckSkipSpace("   A", 'A', "空白の並び + 空白以外 => 空白以外まで移動");
            CheckSkipSpace("\t\f\n\r\vB", 'B', "タブ、フォームフィード、改行、CR、垂直タブ => これらは空白");
            CheckSkipSpace("　C", 'C', "全角の空白 => これも空白");
        }

        private void CheckSkipSpace(String line, Char expectedAfterSkip, String message)
        {
            m_target = new ReadBuffer(line);
            m_target.SkipSpace();
            CheckCurrent(expectedAfterSkip, message);
        }

        /// <summary>
        /// SkipExpected メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void SkipExpected()
        {
            CheckSkipExpected("#", '#', true, "現在位置が予期した文字である => OK");
            CheckSkipExpected("#", '!', false, "現在位置が予期した文字でない => 例外");
            CheckSkipExpected(String.Empty, '#', false, "現在位置が文字列の最後 => 例外");
        }

        private void CheckSkipExpected(String str, Char expected, Boolean success, String message)
        {
            m_target = new ReadBuffer(str);
            try
            {
                m_target.SkipExpected(expected);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// ReadNoneSpace メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadNoneSpace()
        {
            CheckReadNoneSpace(String.Empty, String.Empty, "空行 => 空行");
            CheckReadNoneSpace(" abc", String.Empty, "現在位置が空白 => 空行");
            CheckReadNoneSpace("abc;, ", "abc;,", "現在位置が空白以外 => 次の空白の前までを取得");
            CheckReadNoneSpace("xyz", "xyz", "空白以外で文字列が終了 => 文字列の最後までを取得");
        }

        private void CheckReadNoneSpace(String line, String expected, String message)
        {
            m_target = new ReadBuffer(line);
            String actual = m_target.ReadNoneSpace();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckCurrent(Char expected, String message)
        {
            Char actual = m_target.Current;
            Assert.AreEqual(expected, actual, message);
        }
    }
}
