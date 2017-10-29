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
        /// Current プロパティのテストで、空行の場合です。
        /// </summary>
        [TestMethod]
        public void Current_EmptyLine()
        {
            m_target = new ReadBuffer(String.Empty);
            CheckCurrent(ReadBuffer.EndOfStr, "最初から EndOfLine");
        }

        /// <summary>
        /// Current プロパティのテストで、中身がある場合です。
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
        /// CurrentIndex プロパティのテストです。
        /// </summary>
        [TestMethod]
        public void CurrentIndex()
        {
            m_target = new ReadBuffer("123");
            CheckCurrentIndex(0, "作成時のインデックスは 0");

            m_target.MoveNext();
            CheckCurrentIndex(1, "MoveNext 1 回目: 1");

            m_target.MoveNext();
            CheckCurrentIndex(2, "MoveNext 2 回目: 2");

            m_target.MoveNext();
            CheckCurrentIndex(3, "MoveNext 3 回目: 3");

            m_target.MoveNext();
            CheckCurrentIndex(3, "行末を越えるとそれ以上進まない: 3");
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
        /// SkipToEnd メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void SkipToEnd()
        {
            CheckSkipToEnd(String.Empty, 0, "空文字列の場合 => Current は EndOfStr, CurrentIndex は 0");
            CheckSkipToEnd("123", 3, "内容のある文字列の場合 => Current は EndOfStr, CurrentIndex は Length");
        }

        private void CheckSkipToEnd(String str, Int32 expectedCurrentIndex, String message)
        {
            m_target = new ReadBuffer(str);
            m_target.SkipToEnd();
            CheckCurrent(ReadBuffer.EndOfStr, "Current: " + message);
            CheckCurrentIndex(expectedCurrentIndex, "CurrentIndex: " + message);
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
        /// ReadWhile メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadWhile()
        {
            CheckReadWhile(
                "a12", Char.IsLower, "a", 1,
                "文字列の最初から条件が成り立つ文字を連結した文字列が返される");
            CheckReadWhile(
                "a12", Char.IsDigit, String.Empty, 0,
                "文字列の最初から条件が成り立たない => 空文字列");
            CheckReadWhile(
                "a12", Char.IsLetterOrDigit, "a12", 3,
                "文字列の最後まで条件が成り立つ => 文字列の最後まで");
        }

        private void CheckReadWhile(
            String str, Func<Char, Boolean> condition, String expectedResult,
            Int32 expectedCurrentIndex, String message)
        {
            m_target = new ReadBuffer(str);
            String actual = m_target.ReadWhile(condition);
            Assert.AreEqual(expectedResult, actual, message);
            CheckCurrentIndex(expectedCurrentIndex, message);
        }

        /// <summary>
        /// GetRest メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetRest()
        {
            m_target = new ReadBuffer("abc");
            CheckGetRest("abc", "最初は文字列全部");

            m_target.MoveNext();
            CheckGetRest("bc", "CurrentIndex=1 => 2 文字目から最後まで");

            m_target.MoveNext();
            m_target.MoveNext();
            CheckGetRest(String.Empty, "CurrentIndex が文字列の終わりを越えた位置 => 空文字列");
        }

        private void CheckGetRest(String expected, String message)
        {
            String actual = m_target.GetRest();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// GetToCurrent メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetToCurrent()
        {
            m_target = new ReadBuffer("abc");
            m_target.MoveNext();
            m_target.MoveNext();
            m_target.MoveNext();

            CheckGetToCurrent(
                3, String.Empty,
                "fromIndex と CurrentIndex が同じ => 空文字列");
            CheckGetToCurrent(
                2, "c",
                "CurrentIndex - fromIndex > 0 => fromIndex から CurrentIndex までの文字列");
            CheckGetToCurrent(
                0, "abc",
                "fromIndex=0, CurrentIndex=最後を超えた位置 => 文字列全体");
        }

        private void CheckGetToCurrent(Int32 fromIndex, String expected, String message)
        {
            String actual = m_target.GetToCurrent(fromIndex);
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckCurrent(Char expected, String message)
        {
            Char actual = m_target.Current;
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckCurrentIndex(Int32 expected, String message)
        {
            Int32 actual = m_target.CurrentIndex;
            Assert.AreEqual(expected, actual, message);
        }
    }
}
