using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Utils;

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
        public void Read_AddressRange()
        {
            CheckRead_AddressRange(-1, false, "-1: 最小より小さい => 失敗");
            CheckRead_AddressRange(0, true, "0: ちょうど最小 => 成功");
            CheckRead_AddressRange(65535, true, "65535: ちょうど最大 => 成功");
            CheckRead_AddressRange(65536, false, "65536: 最大より大きい => 失敗");
        }

        private void CheckRead_AddressRange(Int32 address, Boolean success, String message)
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
        public void Write_AddressRange()
        {
            CheckWrite_AddressRange(-1, false, "-1: 最小より小さい => 失敗");
            CheckWrite_AddressRange(0, true, "0: ちょうど最小 => 成功");
            CheckWrite_AddressRange(65535, true, "65535: ちょうど最大 => 成功");
            CheckWrite_AddressRange(65536, false, "65536: 最大より大きい => 失敗");
        }

        private void CheckWrite_AddressRange(Int32 address, Boolean success, String message)
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

        /// <summary>
        /// Write(Int32 startAddress, params UInt16[] values) の単体テストです。
        /// </summary>
        [TestMethod]
        public void Write_Values()
        {
            CheckWrite_Values(
                0x0000, new UInt16[] { 0x1234, 0x2345, 0x3456 },
                true, "0 番地から 3 語書き込み => 成功");
            CheckWrite_Values(
                0xfffe, new UInt16[] { 0x1111, 0x2222, 0x3333 },
                false, "fffe 番地から 3 語書き込み => ffff を超えるので失敗");
        }

        private void CheckWrite_Values(Int32 startAddress, UInt16[] values, Boolean success, String message)
        {
            try
            {
                m_memory.Write(startAddress, values);
                Assert.IsTrue(success, message);
                CheckContents(startAddress, values, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        private void CheckContents(Int32 startAddress, UInt16[] expectedValues, String message)
        {
            expectedValues.ForEach(
                (index, expectedValue) => CheckOneContents(startAddress + index, expectedValue, message));
        }

        private void CheckOneContents(Int32 address, UInt16 expected, String message)
        {
            Word word = m_memory.Read(address);
            UInt16 actual = word.GetAsUnsigned();
            String assertMessage = String.Format("0x{0:x04}: {1}", address, message);
            Assert.AreEqual(expected, actual, assertMessage);
        }
    }
}
