using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

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
            CheckRead("''", true, String.Empty, "空文字列 => ここでは OK、コンストラクタでチェックする");

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
        /// コンストラクタのテストです。
        /// </summary>
        [TestMethod]
        public void Ctor()
        {
            CheckCtor("Contents", true, "Jisx0201 で表せる文字の並び => OK");
            CheckCtor("全角", false, "Jisx0201 で表せない文字がある => 例外");
            CheckCtor(String.Empty, false, "空文字列 => 例外");
        }

        public void CheckCtor(String value, Boolean success, String message)
        {
            try
            {
                StringConstant target = new StringConstant(value);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(
                " !1Aa~",
                WordTest.MakeArray(0x0020, 0x0021, 0x0031, 0x0041, 0x0061, 0x007E),
                "間隔、記号、数字、英大文字、英小文字、7 ビットの最後の文字 '~'");
            CheckGenerateCode(
                @"\¥",
                WordTest.MakeArray(0x005C, 0x005C),
                "バックスラッシュと円記号、JISX0201 だと どちらも 0x005C");
            CheckGenerateCode(
                "｡ｱｧ",
                WordTest.MakeArray(0x00A1, 0x00B1, 0x00A7),
                "半角かな");
        }

        private void CheckGenerateCode(String value, Word[] expectedWords, String message)
        {
            StringConstant target = new StringConstant(value);
            ConstantTest.CheckGenerateCode(target, expectedWords, message);
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
