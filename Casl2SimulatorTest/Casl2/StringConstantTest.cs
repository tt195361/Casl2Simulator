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
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const String DontCare = null;

            CheckParse("'文字定数'", true, "文字定数", "シングルクォート内の文字列を値とする => OK");
            CheckParse("'''a'''", true, "'a'", "シングルクォートは 2 個続けて書く => OK");
            CheckParse("',; '", true, ",; ", "コンマ、セミコロン、スペースも含められる => OK");

            CheckParse("!'", false, DontCare, "開き側のシングルクォートなし => 例外");
            CheckParse("'!", false, DontCare, "閉じ側のシングルクォートなし => 例外");
        }

        private void CheckParse(String str, Boolean success, String expected, String message)
        {
            StringConstant target = ConstantTest.CheckParse(StringConstant.Parse, str, success, message);
            if (success)
            {
                String actual = target.Value;
                Assert.AreEqual(expected, actual, message);
            }
        }
    }
}
