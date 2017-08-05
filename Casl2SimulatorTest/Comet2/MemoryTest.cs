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
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_memory = new Memory();
        }

        /// <summary>
        /// Reset メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Reset()
        {
            const UInt16 NoneZeroValue = 0x55aa;
            Write(m_memory, 0x0000, NoneZeroValue);
            Write(m_memory, 0xffff, NoneZeroValue);

            m_memory.Reset();

            Check(m_memory, 0x0000, 0, "すべてのアドレスの値が 0 になる: 0x0000");
            Check(m_memory, 0xffff, 0, "すべてのアドレスの値が 0 になる: 0xffff");
        }

        /// <summary>
        /// Read メソッドと Write メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ReadWrite()
        {
            Word word1 = new Word(12345);
            Word word2 = new Word(23456);

            CheckReadWrite(0x0000, word1, "アドレス 0x0000");
            CheckReadWrite(0xffff, word2, "アドレス 0xffff");
        }

        private void CheckReadWrite(UInt16 ui16Addr, Word wordWrite, String message)
        {
            // 書き込んだ語が読み出せること。
            Word wordAddr = new Word(ui16Addr);
            m_memory.Write(wordAddr, wordWrite);

            Word wordRead = m_memory.Read(wordAddr);
            WordTest.Check(wordWrite, wordRead, message);
        }

        internal static void Check(Memory mem, UInt16 ui16Addr, UInt16 expected, String message)
        {
            Word wordRead = Read(mem, ui16Addr);
            WordTest.Check(wordRead, expected, message);
        }

        internal static void WriteRange(Memory mem, UInt16 startAddress, params UInt16[] values)
        {
            values.ForEach(
                (index, value) => Write(mem, (UInt16)(startAddress + index), values[index]));
        }

        internal static void Write(Memory mem, UInt16 ui16Addr, UInt16 ui16Value)
        {
            Word wordAddr = new Word(ui16Addr);
            Word wordValue = new Word(ui16Value);
            mem.Write(wordAddr, wordValue);
        }

        private static Word Read(Memory mem, UInt16 ui16Addr)
        {
            Word wordAddr = new Word(ui16Addr);
            return mem.Read(wordAddr);
        }
    }
}
