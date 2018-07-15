using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Common
{
    /// <summary>
    /// <see cref="Word"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class WordTest
    {
        #region Instance Fields
        private Word m_word_0000;
        private Word m_word_7fff;
        private Word m_word_8000;
        private Word m_word_ffff;

        private Word m_target;
        private Word m_sameValue;
        private Word m_differentValue;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_word_0000 = new Word(0x0000);
            m_word_7fff = new Word(0x7fff);
            m_word_8000 = new Word(0x8000);
            m_word_ffff = new Word(0xffff);

            m_target = 1234;
            m_sameValue = 1234;
            m_differentValue = 1357;
        }

        /// <summary>
        /// 符号なし 16 ビットの値を引数に取るコンストラクタをテストします。
        /// </summary>
        [TestMethod]
        public void CtorUnsigned()
        {
            CheckCtorUnsigned(0x0000, "0x0000");
            CheckCtorUnsigned(0x7fff, "0x7fff");
            CheckCtorUnsigned(0x8000, "0x8000");
            CheckCtorUnsigned(0xffff, "0xffff");
        }

        private void CheckCtorUnsigned(UInt16 ui16ValSet, String message)
        {
            Word target = new Word(ui16ValSet);
            UInt16 ui16ValGot = target.GetAsUnsigned();
            Assert.AreEqual(ui16ValSet, ui16ValGot, message);
        }

        /// <summary>
        /// 符号付き 16 ビットの値を引数に取るコンストラクタをテストします。
        /// </summary>
        [TestMethod]
        public void CtorSigned()
        {
            CheckCtorSigned(0, "0");
            CheckCtorSigned(32767, "32767");
            CheckCtorSigned(-32768, "-32768");
            CheckCtorSigned(-1, "-1");
        }

        private void CheckCtorSigned(Int16 i16ValSet, String message)
        {
            Word target = new Word(i16ValSet);
            Int16 i16ValGot = target.GetAsSigned();
            Assert.AreEqual(i16ValSet, i16ValGot, message);
        }

        /// <summary>
        /// <see cref="Word.Equals"/> のテストです。
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            Object obj = new object();

            CheckEquals(null, false, "null => 等しくない");
            CheckEquals(obj, false, "型が違う => 等しくない");
            CheckEquals(m_differentValue, false, "異なる値の Word => 等しくない");
            CheckEquals(m_sameValue, true, "同じ値の Word => 等しい");
        }

        private void CheckEquals(Object obj, Boolean expected, String message)
        {
            Boolean actual = m_target.Equals(obj);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// '==' 演算子のテストです。
        /// </summary>
        [TestMethod]
        public void OperatorEqual()
        {
            CheckOperatorEqual(m_sameValue, true, "同じ値 => true");
            CheckOperatorEqual(m_differentValue, false, "異なる値 => false");
        }

        private void CheckOperatorEqual(Word that, Boolean expected, String message)
        {
            Boolean actual = (m_target == that);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// '!=' 演算子のテストです。
        /// </summary>
        [TestMethod]
        public void OperatorNotEqual()
        {
            CheckOperatorNotEqual(m_sameValue, false, "同じ値 => false");
            CheckOperatorNotEqual(m_differentValue, true, "異なる値 => true");
        }

        private void CheckOperatorNotEqual(Word that, Boolean expected, String message)
        {
            Boolean actual = (m_target != that);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// <see cref="Word.GetAsUnsigned"/> メソッドをテストします。
        /// </summary>
        [TestMethod]
        public void GetAsUnsigned()
        {
            CheckGetAsUnsigned(m_word_0000, 0, "0x0000 => 0");
            CheckGetAsUnsigned(m_word_7fff, 32767, "0x7fff => 32767");
            CheckGetAsUnsigned(m_word_8000, 32768, "0x8000 => 32768");
            CheckGetAsUnsigned(m_word_ffff, 65535, "0xffff => 65535");
        }

        private void CheckGetAsUnsigned(Word target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// <see cref="Word.GetAsSigned"/> メソッドをテストします。
        /// </summary>
        [TestMethod]
        public void GetAsSigned()
        {
            CheckGetAsSigned(m_word_0000, 0, "0x0000 => 0");
            CheckGetAsSigned(m_word_7fff, 32767, "0x7fff => 32767");
            CheckGetAsSigned(m_word_8000, -32768, "0x8000 => -32768");
            CheckGetAsSigned(m_word_ffff, -1, "0xffff => -1");
        }

        private void CheckGetAsSigned(Word target, Int16 expected, String message)
        {
            Int16 actual = target.GetAsSigned();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// <see cref="Word.GetBits"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void GetBits()
        {
            CheckGetBits(0xfffe, 0, 0, 0, "最下位ビットを取得");
            CheckGetBits(0x8000, 15, 15, 1, "最上位ビットを取得");
            CheckGetBits(0xffa5, 7, 0, 0xa5, "下位 8 ビットを取得");
            CheckGetBits(0xc300, 15, 8, 0xc3, "上位 8 ビットを取得");
            CheckGetBits(0xa5a5, 15, 0, 0xa5a5, "16 ビットすべて取得");
        }

        private void CheckGetBits(
            UInt16 value, Int32 fromUpperBit, Int32 toLowerBit, UInt16 expected, String message)
        {
            Word target = new Word(value);
            UInt16 actual = target.GetBits(fromUpperBit, toLowerBit);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// <see cref="Word.IsMinus"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void IsMinus()
        {
            CheckIsMinus(-1, true, "-1 => true");
            CheckIsMinus(-32768, true, "-32768 => true");
            CheckIsMinus(0, false, "0 => false");
            CheckIsMinus(32767, false, "32767 => false");
        }

        private void CheckIsMinus(Int16 i16Val, Boolean expected, String message)
        {
            Word word = new Word(i16Val);
            Boolean actual = word.IsMinus();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// <see cref="Word.IsZero"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void IsZero()
        {
            CheckIsZero(-1, false, "-1 => false");
            CheckIsZero(-32768, false, "-32768 => false");
            CheckIsZero(0, true, "0 => true");
            CheckIsZero(32767, false, "32767 => false");
        }

        private void CheckIsZero(Int16 i16Val, Boolean expected, String message)
        {
            Word word = new Word(i16Val);
            Boolean actual = word.IsZero();
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(Word actual, UInt16 expectedValue, String message)
        {
            Word expected = new Word(expectedValue);
            Check(expected, actual, message);
        }

        internal static void Check(Word actual, Int16 expectedValue, String message)
        {
            Word expected = new Word(expectedValue);
            Check(expected, actual, message);
        }

        internal static void Check(Word expected, Word actual, String message)
        {
            UInt16 expectedValue = expected.GetAsUnsigned();
            UInt16 actualValue = actual.GetAsUnsigned();
            Assert.AreEqual(expectedValue, actualValue, message);
        }

        internal static Word[] MakeArray()
        {
            return new Word[0];
        }

        internal static Word[] MakeArray(params UInt16[] ui16Vals)
        {
            return ui16Vals.Select((ui16Val) => new Word(ui16Val))
                           .ToArray();
        }

        internal static Word[] MakeArray(params Int16[] i16Vals)
        {
            return i16Vals.Select((i16Val) => new Word(i16Val))
                          .ToArray();
        }

        internal static Word[] MakeCountArray(Word word, Int32 count)
        {
            Word[] words = new Word[count];
            count.Times((index) => words[index] = word);
            return words;
        }
    }
}
