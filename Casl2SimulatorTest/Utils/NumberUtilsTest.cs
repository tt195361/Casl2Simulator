using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// <see cref="NumberUtils"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class NumberUtilsTest
    {
        /// <summary>
        /// ToUInt16(Int16) メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ToUInt16FromInt16()
        {
            CheckToUInt16FromInt16(0, 0, "Int16 の 0 (0x0000) => UInt16 でも 0");
            CheckToUInt16FromInt16(32767, 32767, "Int16 の 32767 (0x7fff) => UInt16 でも 32767");
            CheckToUInt16FromInt16(-32768, 32768, "Int16 の -32768 (0x8000) => UInt16 だと 32768");
            CheckToUInt16FromInt16(-1, 65535, "Int16 の -1 (0xffff) => UInt16 だと 65535");
        }

        private void CheckToUInt16FromInt16(Int16 i16Val, UInt16 expected, String message)
        {
            UInt16 actual = NumberUtils.ToUInt16(i16Val);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// ToUInt16(Int32) メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ToUInt16FromInt32()
        {
            CheckToUInt16FromInt32(0, 0, "Int32 の 0 => UInt16 でも 0");
            CheckToUInt16FromInt32(32767, 32767, "Int32 の 32767 (0x00007fff) => UInt16 でも 32767");
            CheckToUInt16FromInt32(32768, 32768, "Int32 の 32768 (0x00008000) => UInt16 でも 32768");
            CheckToUInt16FromInt32(65535, 65535, "Int32 の 65535 (0x0000ffff) => UInt16 でも 65535");
            CheckToUInt16FromInt32(65536, 0, "Int32 の 65536 (0x00010000) => UInt16 だと 0");
            CheckToUInt16FromInt32(-32768, 32768, "Int32 の -32768 (0xffff8000) => UInt16 だと 32768 (0x8000)");
            CheckToUInt16FromInt32(-1, 65535, "Int32 の -1 (0xffffffff) => UInt16 だと 65535 (0xffff)");
        }

        private void CheckToUInt16FromInt32(Int32 i32Val, UInt16 expected, String message)
        {
            UInt16 actual = NumberUtils.ToUInt16(i32Val);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// ToInt16(UInt16) メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ToInt16FromUInt16()
        {
            CheckToInt16FromUInt16(0, 0, "UInt16 の 0 (0x0000) => Int16 でも 0");
            CheckToInt16FromUInt16(32767, 32767, "UInt16 の 32767 (0x7fff) => Int16 でも 32767");
            CheckToInt16FromUInt16(32768, -32768, "UInt16 の 32768 (0x8000) => Int16 だと -32768");
            CheckToInt16FromUInt16(65535, -1, "UInt16 の 65535 (0xffff) => Int16 だと -1");
        }

        private void CheckToInt16FromUInt16(UInt16 ui16Val, Int16 expected, String message)
        {
            Int16 actual = NumberUtils.ToInt16(ui16Val);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// ToInt16(Int32) メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void ToInt16FromInt32()
        {
            CheckToInt16FromInt32(0, 0, "Int32 の 0 (0x00000000) => Int16 でも 0");
            CheckToInt16FromInt32(32767, 32767, "Int32 の 32767 (0x00007fff) => Int16 でも 32767");
            CheckToInt16FromInt32(32768, -32768, "Int32 の 32768 (0x00008000) => Int16 だと -32768 (0x8000)");
            CheckToInt16FromInt32(65535, -1, "Int32 の 65535 (0x0000ffff) => Int16 だと -1 (0xffff)");
            CheckToInt16FromInt32(65536, 0, "Int32 の 65536 (0x00010000) => Int16 だと 0");
            CheckToInt16FromInt32(-32768, -32768, "Int32 の -32768 (0xffff8000) => Int16 でも -32768 (0x8000)");
            CheckToInt16FromInt32(-1, -1, "Int32 の -1 (0xffffffff) => Int16 でも -1 (0xffff)");
        }

        private void CheckToInt16FromInt32(Int32 i32Val, Int16 expected, String message)
        {
            Int16 actual = NumberUtils.ToInt16(i32Val);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// CheckInt16Overflow メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CheckInt16Overflow()
        {
            CheckCheckInt16Overflow(-32769, true, "-32769 (0xffff7fff) => オーバーフロー");
            CheckCheckInt16Overflow(-32768, false, "-32768 (0xffff8000) => OK");
            CheckCheckInt16Overflow(32767, false, "32767 (0x00007fff) => OK");
            CheckCheckInt16Overflow(32768, true, "32768 (0x00008000) => オーバーフロー");
        }

        private void CheckCheckInt16Overflow(Int32 i32Val, Boolean expected, String message)
        {
            Boolean actual = NumberUtils.CheckInt16Overflow(i32Val);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// CheckUInt16Overflow メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void CheckUInt16Overflow()
        {
            CheckCheckUInt16Overflow(-1, true, "-1 (0xffffffff) => オーバーフロー");
            CheckCheckUInt16Overflow(0, false, "0 (0x00000000) => OK");
            CheckCheckUInt16Overflow(65535, false, "65535 (0x0000ffff) => OK");
            CheckCheckUInt16Overflow(65536, true, "65536 (0x00010000) => オーバーフロー");
        }

        private void CheckCheckUInt16Overflow(Int32 i32Val, Boolean expected, String message)
        {
            Boolean actual = NumberUtils.CheckUInt16Overflow(i32Val);
            Assert.AreEqual(expected, actual);
        }
    }
}
