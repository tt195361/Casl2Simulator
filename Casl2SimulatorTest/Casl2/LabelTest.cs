using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Label クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelTest
    {
        #region Fields
        internal const String ValidLabel = "LBL001";
        internal const String InvalidLabel = "不正なラベル";

        private const String DontCare = null;
        #endregion

        /// <summary>
        /// ParseLine メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseLine()
        {
            CheckParseLine(String.Empty, true, null, "空文字 => ラベルなし");
            CheckParseLine(" ", true, null, "空白 => ラベルなし");

            CheckParseLine("A", true, "A", "半角英大文字 1 文字");
            CheckParseLine("A2B4C6D8", true, "A2B4C6D8", "半角英大文字とそれに続く半角数字か半角英大文字で 8 文字");

            CheckParseLine("A23456789", false, DontCare, "文字数が 8 文字より多い => エラー");
            CheckParseLine("Ａ", false, DontCare, "先頭が半角英大文字でない => エラー");
            CheckParseLine("A_", false, DontCare, "以降に半角英大文字か数字以外 (_) => エラー");
            CheckParseLine("Ab", false, DontCare, "以降に半角英大文字か数字以外 (b) => エラー");
            CheckParseLine("AＢ", false, DontCare, "以降に半角英大文字か数字以外 (Ｂ) => エラー");
            CheckParseLine("A８", false, DontCare, "以降に半角英大文字か数字以外 (８) => エラー");
            CheckParseLine("GR0", false, DontCare, "予約語 => エラー");
        }

        private void CheckParseLine(String str, Boolean success, String expected, String message)
        {
            CheckParse(Label.ParseLine, str, success, expected, message);
        }

        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            CheckParseOperand("L001", true, "L001", "ラベルで文字列の終わり => 文字列の終わりの前まで解釈");
            CheckParseOperand("L001 ", true, "L001", "ラベルに続いて空白 => 空白の前まで解釈");
            CheckParseOperand("L001,", true, "L001", "ラベルに続いてコンマ => コンマの前まで解釈");

            CheckParseOperand(String.Empty, false, DontCare, "空文字 => ラベルがない => 例外");
            CheckParseOperand(" ", false, DontCare, "空白 => ラベルがない => 例外");
            CheckParseOperand("001", false, DontCare, "不正なラベル => 例外");
        }

        private void CheckParseOperand(String str, Boolean success, String expected, String message)
        {
            CheckParse(Label.ParseOperand, str, success, expected, message);
        }

        private void CheckParse(
            Func<ReadBuffer, String> checkFunc, String str, Boolean success, String expected, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                String actual = checkFunc(buffer);
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
