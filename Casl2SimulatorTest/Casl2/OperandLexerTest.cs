using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Utils;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// OperandLexer クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class OperandLexerTest
    {
        /// <summary>
        /// MoveNext メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void MoveNext()
        {
            CheckMoveNext(
                String.Empty,
                TestUtils.MakeArray(Token.MakeEndOfToken()),
                "空文字列 => EndOfToken");
            CheckMoveNext(
                " ",
                TestUtils.MakeArray(Token.MakeEndOfToken()),
                "空白 => EndOfToken");

            CheckMoveNext(
                "12345,#ABCD,'STR',=GR3,LABEL",
                TestUtils.MakeArray(
                    Token.MakeDecimalConstant(12345),
                    Token.MakeComma(),
                    Token.MakeHexaDecimalConstant(0xABCD),
                    Token.MakeComma(),
                    Token.MakeStringConstant("STR"),
                    Token.MakeComma(),
                    Token.MakeEqualSign(),
                    Token.MakeRegisterName("GR3"),
                    Token.MakeComma(),
                    Token.MakeLabel("LABEL"),
                    Token.MakeEndOfToken()),
                "それぞれの TokenType を解釈できる。最後は EndOfToken が返される。");

            CheckMoveNext(
                "$",
                TestUtils.MakeArray<Token>(null),
                "Token として解釈できない場合は、例外が投げられる。");
        }

        private void CheckMoveNext(String str, Token[] expectedArray, String message)
        {
            OperandLexer lexer = MakeFrom(str);
            expectedArray.ForEach((expected) => CheckMoveNextOne(lexer, expected, message));
        }

        private void CheckMoveNextOne(OperandLexer lexer, Token expected, String message)
        {
            try
            {
                lexer.MoveNext();
                Token actual = lexer.CurrentToken;
                Assert.IsNotNull(expected, message);
                TokenTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }

        internal static OperandLexer MakeFrom(String str)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            OperandLexer lexer = new OperandLexer(buffer);
            return lexer;
        }
    }
}
