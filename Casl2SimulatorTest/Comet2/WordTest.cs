using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Word クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class WordTest
    {
        #region Fields
        private Word m_word_0000;
        private Word m_word_7fff;
        private Word m_word_8000;
        private Word m_word_ffff;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_word_0000 = new Word(0x0000);
            m_word_7fff = new Word(0x7fff);
            m_word_8000 = new Word(0x8000);
            m_word_ffff = new Word(0xffff);
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
        /// GetAsUnsigned メソッドをテストします。
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
        /// GetAsSigned メソッドをテストします。
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
    }
}
