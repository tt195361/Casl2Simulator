using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Token"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class TokenTest
    {
        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            CheckToString(
                Token.MakeEndOfToken(), "オペランドの終わり",
                "EndOfToken => オペランドの終わり");
            CheckToString(
                Token.MakeDecimalConstant(12345), "10 進定数: 12345",
                "DecimalConstant => 説明と 10 進表記の値");
            CheckToString(
                Token.MakeHexaDecimalConstant(0x048C), "16 進定数: #048C",
                "HexaDecimalConstant => 説明と 16 進表記の 4 桁の値");
            CheckToString(
                Token.MakeStringConstant("ABC"), "文字定数: 'ABC'",
                "StringConstant => 説明と '文字列'");
            CheckToString(
                Token.MakeRegisterName("GR6"), "レジスタ名: GR6",
                "RegisterName => 説明とレジスタ名");
            CheckToString(
                Token.MakeLabel("LABEL"), "ラベル: LABEL",
                "Label => 説明とラベル");
            CheckToString(
                Token.MakeComma(), "','",
                "Comma => ','");
            CheckToString(
                Token.MakeEqualSign(), "'='",
                "EqualSign => '='");
        }

        private void CheckToString(Token target, String expected, String message)
        {
            String actual = target.ToString();
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(Token expected, Token actual, String message)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, message);
            }
            else
            {
                Assert.AreEqual(expected.Type, actual.Type, "Type: " + message);
                Assert.AreEqual(expected.I32Value, actual.I32Value, "I32Value: " + message);
                Assert.AreEqual(expected.StrValue, actual.StrValue, "StrValue: " + message);
            }
        }
    }
}
