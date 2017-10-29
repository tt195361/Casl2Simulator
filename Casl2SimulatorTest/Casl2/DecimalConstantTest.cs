using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// DecimalConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class DecimalConstantTest
    {
        #region Fields
        private const Int32 DontCare = 0;
        #endregion

        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            CheckIsStart('-', true, "マイナス => true: 10 進定数の最初の文字");
            CheckIsStart('5', true, "数字 => true: 10 進定数の最初の文字");
            CheckIsStart('#', false, "マイナスと数字以外 => false: 10 進定数の最初の文字でない");
        }

        private void CheckIsStart(Char firstChar, Boolean expected, String message)
        {
            Boolean actual = DecimalConstant.IsStart(firstChar);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Read メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Read()
        {
            CheckRead("123", true, 123, "正の数字");
            CheckRead("-246", true, -246, "負の数字");
            CheckRead("0", true, 0, "零");

            CheckRead("13579abc", true, 13579, "数字として解釈できるところまで解釈する");
            CheckRead("abc", false, DontCare, "最初が数字でない => 例外");
            CheckRead("-abc", false, DontCare, "'-' の後ろが数字でない => 例外");
            CheckRead("1234567", true, 1234567, "Read は範囲チェックしない => OK");
        }

        private void CheckRead(String str, Boolean success, Int32 expected, String message)
        {
            Int32 actual = ConstantTest.CheckRead(DecimalConstant.Read, str, success, message);
            if (success)
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        /// <summary>
        /// コンストラクタのテストです。
        /// </summary>
        [TestMethod]
        public void Ctor()
        {
            CheckCtor(-32769, false, "最小値より小さい => 例外");
            CheckCtor(-32768, true, "ちょうど最小値 => OK");
            CheckCtor(32767, true, "ちょうど最大値 => OK");
            CheckCtor(32768, false, "最大値より大きい => 例外");
        }

        private void CheckCtor(Int32 value, Boolean success, String message)
        {
            try
            {
                DecimalConstant notUsed = new DecimalConstant(value);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(DecimalConstant expected, DecimalConstant actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
