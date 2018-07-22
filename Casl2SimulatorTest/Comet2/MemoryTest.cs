using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="Memory"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class MemoryTest
    {
        #region Instance Fields
        private Memory m_memory;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_memory = new Memory();
        }

        /// <summary>
        /// <see cref="Memory.Clear"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Clear()
        {
            const UInt16 NoneZeroValue = 0x55aa;
            m_memory.Write(0x0000, NoneZeroValue);
            m_memory.Write(0xffff, NoneZeroValue);

            m_memory.Clear();

            Check(m_memory, 0x0000, 0, "すべてのアドレスの値が 0 になる: 0x0000");
            Check(m_memory, 0xffff, 0, "すべてのアドレスの値が 0 になる: 0xffff");
        }

        /// <summary>
        /// <see cref="Memory.Read"/> メソッドと <see cref="Memory.Write"/> メソッドの単体テストです。
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

        /// <summary>
        /// <see cref="Memory.WriteRange"/> メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void WriteRange()
        {
            CheckWriteRange(
                0x0000, new UInt16[] { 0x1111, 0x2222, 0x3333 },
                "0x0000 から 0x0002 まで 3 バイト書き込み");
            CheckWriteRange(
                0xfffd, new UInt16[] { 0x4444, 0x5555, 0x6666 },
                "0xfffd から 0xffff まで 3 バイト書き込み");
            CheckWriteRange(
                0xfffe, new UInt16[] { 0x7777, 0x8888, 0x9999, 0xaaaa },
                "0xfffe から 0x0001 まで 4 バイト書き込み、例外は発生せず最初に戻る");
        }

        private void CheckWriteRange(UInt16 ui16StartAddr, UInt16[] ui16Values, String message)
        {
            m_memory.WriteRange(ui16StartAddr, ui16Values);
            CheckRange(ui16StartAddr, ui16Values, message);
        }

        private void CheckRange(UInt16 ui16StartAddr, UInt16[] ui16Values, String message)
        {
            m_memory.ForEach(
                ui16StartAddr, ui16Values, (ui16Addr, ui16Value) => Check(ui16Addr, ui16Value, message));
        }

        private void Check(UInt16 ui16Addr, UInt16 expected, String message)
        {
            Check(m_memory, ui16Addr, expected, message);
        }

        internal static void Check(Memory mem, UInt16 ui16Addr, UInt16 expected, String message)
        {
            Word wordRead = mem.Read(ui16Addr);
            WordTest.Check(wordRead, expected, message);
        }
    }
}
