using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// StringConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class StringConstantTest
    {
        /// <summary>
        /// IsStart メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsStart()
        {
            CheckIsStart('\'', true, "' => true: 文字定数の最初の文字");
            CheckIsStart('!', false, "' 以外 => false: 文字定数の最初の文字でない");
        }

        private void CheckIsStart(Char firstChar, Boolean expected, String message)
        {
            Boolean actual = StringConstant.IsStart(firstChar);
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Read メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Read()
        {
            const String DontCare = null;

            CheckRead("'文字定数'", true, "文字定数", "シングルクォート内の文字列を値とする => OK");
            CheckRead("'''a'''", true, "'a'", "シングルクォートは 2 個続けて書く => OK");
            CheckRead("',; '", true, ",; ", "コンマ、セミコロン、スペースも含められる => OK");

            CheckRead("!'", false, DontCare, "開き側のシングルクォートなし => 例外");
            CheckRead("'!", false, DontCare, "閉じ側のシングルクォートなし => 例外");
            CheckRead("'!''", false, DontCare, "シングルクォート 2 個続きで終わり => 例外");
        }

        private void CheckRead(String str, Boolean success, String expected, String message)
        {
            String actual = ConstantTest.CheckRead(StringConstant.Read, str, success, message);
            if (success)
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        /// <summary>
        /// ValueToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ValueToString()
        {
            CheckValueToString("abc", "'abc'", "' と ' で囲まれる");
            CheckValueToString("'a'", "'''a'''", "' は '' に変換される");
        }

        private void CheckValueToString(String value, String expected, String message)
        {
            String actual = StringConstant.ValueToString(value);
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(StringConstant expected, StringConstant actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
