using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// WordCount クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class WordCountTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Int32 DontCare = 0;

            CheckParse(
                String.Empty, false, DontCare,
                "オペランドなし => エラー, 語数が必要");
            CheckParse(
                "-1", false, DontCare,
                "下限より小さい => エラー");
            CheckParse(
                "0", true, 0,
                "ちょうど下限 => OK");
            CheckParse(
                "65535", true, 65535,
                "ちょうど上限 => OK");
            CheckParse(
                "65536", false, DontCare,
                "上限より大きい => エラー");
            CheckParse(
                "#ABCD", false, DontCare,
                "10 進定数以外 => エラー, 10 進定数で指定する");
            CheckParse(
                "123,'ABC'", true, 123,
                "10 進定数のあとにまだ文字がある => ここでは OK");
        }

        private void CheckParse(
            String str, Boolean success, Int32 expectedValue, String message)
        {
            WordCount actual = OperandTest.CheckParse(WordCount.Parse, str, success, message);
            if (success)
            {
                WordCount expected = WordCount.MakeForUnitTest(expectedValue);
                Check(expected, actual, message);
            }
        }

        internal static void Check(WordCount expected, WordCount actual, String message)
        {
            Assert.AreEqual(expected.Value, actual.Value, "Value: " + message);
        }
    }
}
