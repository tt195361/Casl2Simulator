using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// NumericConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class NumericConstantTest
    {
        #region Fields
        private const Int32 DontCare = 0;
        #endregion

        /// <summary>
        /// ParseDecimal メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseDecimal()
        {
            CheckParseDecimal("123", true, 123, "正の数字");
            CheckParseDecimal("-246", true, -246, "負の数字");
            CheckParseDecimal("0", true, 0, "零");

            CheckParseDecimal("13579abc", true, 13579, "数字として解釈できるところまで解釈する");
            CheckParseDecimal("abc", false, DontCare, "最初が数字でない => 例外");
            CheckParseDecimal("-abc", false, DontCare, "'-' の後ろが数字でない => 例外");

            CheckParseDecimal("-32769", false, DontCare, "範囲の最小より小さい => 例外");
            CheckParseDecimal("-32768", true, -32768, "範囲の最小ちょうど => OK");
            CheckParseDecimal("32767", true, 32767, "範囲の最大ちょうど => OK");
            CheckParseDecimal("32768", false, DontCare, "範囲の最大より大きい => 例外");
        }

        private void CheckParseDecimal(String str, Boolean success, Int32 expected, String message)
        {
            CheckParse(NumericConstant.ParseDecimal, str, success, expected, message);
        }

        /// <summary>
        /// ParseHexaDecimal メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseHexaDecimal()
        {
            CheckParseHexaDecimal("#5AC3", true, 0x5AC3, "'#' で始まり 4 桁の 16 進数が続く => OK");

            CheckParseHexaDecimal("!", false, DontCare, "'#' で始まらない => 例外");
            CheckParseHexaDecimal("#123", false, DontCare, "16 進数が 3 桁しかない => 例外");
            CheckParseHexaDecimal("#12345", false, DontCare, "16 進数が 5 桁ある => 例外");
            CheckParseHexaDecimal("#abcd", false, DontCare, "小文字は 16 進数でない => 例外");
        }

        private void CheckParseHexaDecimal(String str, Boolean success, Int32 expected, String message)
        {
            CheckParse(NumericConstant.ParseHexaDecimal, str, success, expected, message);
        }

        private void CheckParse(
            Func<ReadBuffer, NumericConstant> parseFunc,
            String str, Boolean success, Int32 expected, String message)
        {
            NumericConstant target = ConstantTest.CheckParse(parseFunc, str, success, message);
            if (success)
            {
                Int32 actual = target.Value;
                Assert.AreEqual(expected, actual, message);
            }
        }
    }
}
