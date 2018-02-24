using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// <see cref="Jisx0201Utils"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class Jisx0201UtilsTest
    {
        /// <summary>
        /// ToJisx0201Bytes メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToJisx0201Bytes()
        {
            CheckToJisx0201Bytes("ABCxyz", MakeByteArray(0x41, 0x42, 0x43, 0x78, 0x79, 0x7A), "英文字");
            CheckToJisx0201Bytes("ｱｲｳﾜｦﾝ", MakeByteArray(0xB1, 0xB2, 0xB3, 0xDC, 0xA6, 0XDD), "半角カナ");
        }

        private Byte[] MakeByteArray(params Byte[] bytes)
        {
            return TestUtils.MakeArray(bytes);
        }

        private void CheckToJisx0201Bytes(String str, Byte[] expected, String message)
        {
            Byte[] actual = Jisx0201Utils.ToJisx0201Bytes(str);
            TestUtils.CheckEnumerable(expected, actual, message);
        }

        /// <summary>
        /// ToJisx0201Byte メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToJisx0201Byte()
        {
            const Byte DontCare = 0x00;

            CheckToJisx2010(' ', true, 0x20, "' ' (間隔) => 20");
            CheckToJisx2010('0', true, 0x30, "'0' (数字の零) => 30");
            CheckToJisx2010('@', true, 0x40, "'@' (アットマーク) => 40");
            CheckToJisx2010('P', true, 0x50, "'P' (大文字のピー) => 50");
            CheckToJisx2010('`', true, 0x60, "'`' (バッククォート) => 60");
            CheckToJisx2010('p', true, 0x70, "'p' (小文字のピー) => 70");
            CheckToJisx2010('~', true, 0x7E, "'~' (チルダ) => 7E");

            CheckToJisx2010('｡', true, 0xA1, "'｡' (半角カナの '。') => A1");
            CheckToJisx2010('ｧ', true, 0xA7, "'ｧ' (半角の小さいア) => A7");
            CheckToJisx2010('ｱ', true, 0xB1, "'ｱ' (半角のア) => B1");
            CheckToJisx2010('ﾝ', true, 0xDD, "'ﾝ' (半角のン) => DD");
            CheckToJisx2010('ﾟ', true, 0xDF, "'ﾟ' (半角の°) => DF");

            CheckToJisx2010('¥', true, 0x5C, "'¥' (円記号) => 5C");
            CheckToJisx2010('\\', true, 0x5C, @"'\' (バックスラッシュ) => 円記号と同じ 5C");

            CheckToJisx2010('あ', false, DontCare, "'あ' (全角の 'あ') => Jisx0201 にはない");
        }

        private void CheckToJisx2010(Char c, Boolean success, Byte expected, String message)
        {
            try
            {
                Byte actual = Jisx0201Utils.ToJisx0201Byte(c);
                Assert.IsTrue(success, message);
                Assert.AreEqual(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
