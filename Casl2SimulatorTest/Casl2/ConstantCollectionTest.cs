using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// ConstantCollection クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ConstantCollectionTest
    {
        #region Fields
        private static readonly TokenType DontCare = null;
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            CheckParse(
                "12345",
                ConstantTest.MakeArray(new DecimalConstant(12345)),
                TokenType.EndOfToken,
                "10 進定数");
            CheckParse(
                "#ABCD",
                ConstantTest.MakeArray(new HexaDecimalConstant(0xABCD)),
                TokenType.EndOfToken,
                "16 進定数");
            CheckParse(
                "'abc'",
                ConstantTest.MakeArray(new StringConstant("abc")),
                TokenType.EndOfToken,
                "文字定数");
            CheckParse(
                "L001",
                ConstantTest.MakeArray(new AddressConstant("L001")),
                TokenType.EndOfToken,
                "アドレス定数");
            CheckParse(
                "12345,#ABCD,'StrConst',L001",
                ConstantTest.MakeArray(
                    new DecimalConstant(12345),
                    new HexaDecimalConstant(0xABCD),
                    new StringConstant("StrConst"),
                    new AddressConstant("L001")),
                TokenType.EndOfToken,
                "定数の並び");

            CheckParse(
                String.Empty, null, DontCare,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParse(
                "123,", null, DontCare,
                "コンマに続く定数なし => エラー, コンマがあれば続いて定数が必要");
            CheckParse(
                "'abc'123",
                ConstantTest.MakeArray(new StringConstant("abc")),
                TokenType.DecimalConstant,
                "区切りのコンマがない => コンマがなければ、解釈はそこまで");
        }

        private static void CheckParse(
            String str, Constant[] expectedConstants, TokenType expectedLeftTokenType, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            OperandLexer lexer = new OperandLexer(buffer);
            lexer.MoveNext();
            try
            {
                ConstantCollection actualConstants = ConstantCollection.Parse(lexer);

                Assert.IsNotNull(expectedConstants, message);
                TestUtils.CheckEnumerable(expectedConstants, actualConstants, ConstantTest.Check, message);

                TokenType actualLeftTokenType = lexer.CurrentToken.Type;
                Assert.AreEqual(expectedLeftTokenType, actualLeftTokenType, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedConstants, message);
            }
        }
    }
}
