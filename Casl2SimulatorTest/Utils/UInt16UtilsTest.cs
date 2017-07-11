using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Utils
{
    /// <summary>
    /// UInt16Utils クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class UInt16UtilsTest
    {
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
                const UInt16 DontCare = 0;
                UInt16 notUsed = UInt16Utils.GetBits(DontCare, fromUpperBit, toLowerBit);
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
            UInt16 ui16Val, Int16 fromUpperBit, Int16 toLowerBit, UInt16 expected, String message)
        {
            UInt16 actual = UInt16Utils.GetBits(ui16Val, fromUpperBit, toLowerBit);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// SetBits メソッドが返す値をテストします。
        /// </summary>
        [TestMethod]
        public void SetBits_Result()
        {
            CheckSetBits_Result(0xffff, 0, 0, 0, 0xfffe, "最下位ビットに 0 を設定");
            CheckSetBits_Result(0x0000, 0, 0, 1, 0x0001, "最下位ビットに 1 を設定");
            CheckSetBits_Result(0xffff, 15, 15, 0, 0x7fff, "最上位ビットに 0 を設定");
            CheckSetBits_Result(0x0000, 15, 15, 1, 0x8000, "最上位ビットに 1 を設定");
            CheckSetBits_Result(0xffff, 7, 0, 0xa5, 0xffa5, "下位 8 ビットに値を設定");
            CheckSetBits_Result(0xffff, 15, 8, 0xa5, 0xa5ff, "上位 8 ビットに値を設定");
            CheckSetBits_Result(0xffff, 15, 0, 0xa5a5, 0xa5a5, "16 ビットすべてに値を設定");
            CheckSetBits_Result(
                0x0000, 8, 8, 0xffff, 0x0100,
                "設定する値が指定の範囲より大きい => 指定の範囲だけ設定");
        }

        private void CheckSetBits_Result(
            UInt16 ui16Val, Int16 fromUpperBit, Int16 toLowerBit, UInt16 ui16ValToSet,
            UInt16 expected, String message)
        {
            UInt16 actual = UInt16Utils.SetBits(ui16Val, fromUpperBit, toLowerBit, ui16ValToSet);
            Assert.AreEqual(expected, actual, message);
        }
    }
}
