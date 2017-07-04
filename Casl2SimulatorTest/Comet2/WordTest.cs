using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
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

        /// <summary>
        /// GetBits メソッドの引数をテストします。
        /// </summary>
        [TestMethod]
        public void GetBits_Args()
        {
            CheckGetBits_Args(-1, 0, false, "fromUpperBit = -1: 最小値より小さい => 失敗");
            CheckGetBits_Args(0, 0, true, "fromUpperBit = 0: ちょうど最小値 => 成功");
            CheckGetBits_Args(15, 0, true, "fromUpperBit = 15: ちょうど最大値 => 成功");
            CheckGetBits_Args(16, 0, false, "fromUpperBit = 16: 最大値より大きい => 失敗");

            CheckGetBits_Args(15, -1, false, "toLowerBit = -1: 最小値より小さい => 失敗");
            CheckGetBits_Args(15, 0, true, "toLowerBit = 0: ちょうど最小値 => 成功");
            CheckGetBits_Args(15, 15, true, "toLowerBit = 15: ちょうど最大値 => 成功");
            CheckGetBits_Args(15, 16, false, "toLowerBit = 16: 最大値より大きい => 失敗");

            CheckGetBits_Args(8, 7, true, "fromUpperBits > toLowerBit => 成功");
            CheckGetBits_Args(7, 7, true, "fromUpperBits == toLowerBit => 成功");
            CheckGetBits_Args(7, 8, false, "fromUpperBits < toLowerBit => 失敗");
        }

        private void CheckGetBits_Args(Int32 fromUpperBit, Int32 toLowerBit, Boolean success, String message)
        {
            try
            {
                UInt16 notUsed = m_word_0000.GetBits(fromUpperBit, toLowerBit);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// GetBits メソッドが返す値をテストします。
        /// </summary>
        [TestMethod]
        public void GetBits_Result()
        {
            CheckGetBits_Result(0xfffe, 0, 0, 0, "最下位ビットを取得");
            CheckGetBits_Result(0x8000, 15, 15, 1, "最上位ビットを取得");
            CheckGetBits_Result(0xffa5, 7, 0, 0xa5, "下位 8 ビットを取得");
            CheckGetBits_Result(0xc300, 15, 8, 0xc3, "上位 8 ビットを取得");
            CheckGetBits_Result(0xa5a5, 15, 0, 0xa5a5, "16 ビットすべて取得");
        }

        private void CheckGetBits_Result(
            UInt16 value, Int16 fromUpperBit, Int16 toLowerBit, UInt16 expected, String message)
        {
            Word target = new Word(value);
            UInt16 actual = target.GetBits(fromUpperBit, toLowerBit);
            Assert.AreEqual(expected, actual, message);
        }
    }
}
