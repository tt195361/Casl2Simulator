using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// CharUtils クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class CharUtilsTest
    {
        /// <summary>
        /// IsHankakuUpper メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsHankakuUpper()
        {
            CheckIsHankakuUpper('A', true, "半角の大文字 => true");
            CheckIsHankakuUpper('a', false, "半角だが小文字 => false");
            CheckIsHankakuUpper('Ａ', false, "大文字だが全角 => false");
        }

        private void CheckIsHankakuUpper(Char c, Boolean expected, String message)
        {
            Boolean actual = CharUtils.IsHankakuUpper(c);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// IsHankakuDigit メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsHankakuDigit()
        {
            CheckIsHankakuDigit('1', true, "半角の数字 => true");
            CheckIsHankakuDigit('a', false, "半角だが数字でない => false");
            CheckIsHankakuDigit('１', false, "数字だが全角 => false");
        }

        private void CheckIsHankakuDigit(Char c, Boolean expected, String message)
        {
            Boolean actual = CharUtils.IsHankakuDigit(c);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// IsHankakuHexDigit メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsHankakuHexDigit()
        {
            CheckIsHankakuHexDigit('0', true, "'0' => true");
            CheckIsHankakuHexDigit('1', true, "'1' => true");
            CheckIsHankakuHexDigit('2', true, "'2' => true");
            CheckIsHankakuHexDigit('3', true, "'3' => true");
            CheckIsHankakuHexDigit('4', true, "'4' => true");
            CheckIsHankakuHexDigit('5', true, "'5' => true");
            CheckIsHankakuHexDigit('6', true, "'6' => true");
            CheckIsHankakuHexDigit('7', true, "'7' => true");
            CheckIsHankakuHexDigit('8', true, "'8' => true");
            CheckIsHankakuHexDigit('9', true, "'9' => true");
            CheckIsHankakuHexDigit('A', true, "'A' => true");
            CheckIsHankakuHexDigit('B', true, "'B' => true");
            CheckIsHankakuHexDigit('C', true, "'C' => true");
            CheckIsHankakuHexDigit('D', true, "'D' => true");
            CheckIsHankakuHexDigit('E', true, "'E' => true");
            CheckIsHankakuHexDigit('F', true, "'F' => true");

            CheckIsHankakuHexDigit('１', false, "全角の数字 => false");
            CheckIsHankakuHexDigit('Ａ', false, "全角の数字 => false");
            CheckIsHankakuHexDigit('G', false, "半角だが A..F 以外 => false");
            CheckIsHankakuHexDigit('a', false, "半角小文字 => false");
        }

        private void CheckIsHankakuHexDigit(Char c, Boolean expected, String message)
        {
            Boolean actual = CharUtils.IsHankakuHexDigit(c);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// ToDigit メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToDigit()
        {
            const Int32 DontCare = 0;

            CheckToDigit('9', 10, true, 9, "10 進数の '9' => 9");
            CheckToDigit('F', 16, true, 15, "16 進数の 'F' => 15");
            CheckToDigit('A', 10, false, DontCare, "'A' は 10 進数の文字でない => 例外");
        }

        private void CheckToDigit(Char c, Int32 fromBase, Boolean success, Int32 expected, String message)
        {
            try
            {
                Int32 actual = CharUtils.ToDigit(c, fromBase);
                Assert.IsTrue(success, message);
                Assert.AreEqual(expected, actual, message);
            }
            catch (FormatException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// ToPrintable メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToPrintable()
        {
            CheckToPrintable('a', "a", "印字できる文字 => 文字列に変換");
            CheckToPrintable('\n', @"\u000A", "コントロール文字 => 16 進で表示");
            CheckToPrintable('\xffff', @"\uFFFF", "印字できない文字 => 16 進で表示");
        }

        private void CheckToPrintable(Char c, String expected, String message)
        {
            String actual = CharUtils.ToPrintable(c);
            Assert.AreEqual(expected, actual, message);
        }
    }
}
