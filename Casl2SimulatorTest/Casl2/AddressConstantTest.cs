using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// StringConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AddressConstantTest
    {
        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const String DontCare = null;

            CheckParse("LABEL", true, "LABEL", "ラベルのみ => OK");
            CheckParse("LABEL ", true, "LABEL", "ラベルの後ろに空白 => OK、ラベルのみ取得");
            CheckParse("LABEL,", true, "LABEL", "ラベルの後ろにコンマ => OK、ラベルのみ取得");
            CheckParse("L$", false, DontCare, "不正なラベル => 例外");
        }

        private void CheckParse(String str, Boolean success, String expected, String message)
        {
            AddressConstant target = ConstantTest.CheckParse(AddressConstant.Parse, str, success, message);
            if (success)
            {
                String actual = target.Label;
                Assert.AreEqual(expected, actual, message);
            }
        }
    }
}
