using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Memory クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MemoryTest
    {
        #region Fields
        private Memory m_memory;
        private Word m_word1;
        private Word m_word2;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_memory = new Memory();
            m_word1 = new Word(12345);
            m_word2 = new Word(23456);
        }

        /// <summary>
        /// Read メソッドと Write メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ReadWrite()
        {
            CheckReadWrite(0, m_word1, "アドレス 0");
            CheckReadWrite(65535, m_word2, "アドレス 65535");
        }

        private void CheckReadWrite(Int32 address, Word wordWrite, String message)
        {
            // 書き込んだ語が読み出せること。
            m_memory.Write(address, wordWrite);
            Word wordRead = m_memory.Read(address);
            Assert.AreEqual(wordWrite, wordRead, message);
        }

        /// <summary>
        /// Read メソッドの引数 address の範囲をテストします。
        /// </summary>
        [TestMethod]
        public void ReadAddressRange()
        {
            CheckReadAddressRange(-1, false, "-1: 最小より小さい => 失敗");
            CheckReadAddressRange(0, true, "0: ちょうど最小 => 成功");
            CheckReadAddressRange(65535, true, "65535: ちょうど最大 => 成功");
            CheckReadAddressRange(65536, false, "65536: 最大より大きい => 失敗");
        }

        private void CheckReadAddressRange(Int32 address, Boolean success, String message)
        {
            try
            {
                Word notUsed = m_memory.Read(address);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// Write メソッドの引数 address の範囲をテストします。
        /// </summary>
        [TestMethod]
        public void WriteAddressRange()
        {
            CheckWriteAddressRange(-1, false, "-1: 最小より小さい => 失敗");
            CheckWriteAddressRange(0, true, "0: ちょうど最小 => 成功");
            CheckWriteAddressRange(65535, true, "65535: ちょうど最大 => 成功");
            CheckWriteAddressRange(65536, false, "65536: 最大より大きい => 失敗");
        }

        private void CheckWriteAddressRange(Int32 address, Boolean success, String message)
        {
            try
            {
                m_memory.Write(address, m_word1);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
