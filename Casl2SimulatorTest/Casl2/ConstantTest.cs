using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Constant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ConstantTest
    {
        #region Fields
        private static readonly TokenType DontCare = null;
        #endregion

        /// <summary>
        /// ParseList メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseList()
        {
            CheckParseList(
                "12345",
                MakeArray(new DecimalConstant(12345)),
                TokenType.EndOfToken,
                "10 進定数");
            CheckParseList(
                "#ABCD",
                MakeArray(new HexaDecimalConstant(0xABCD)),
                TokenType.EndOfToken,
                "16 進定数");
            CheckParseList(
                "'abc'",
                MakeArray(new StringConstant("abc")),
                TokenType.EndOfToken,
                "文字定数");
            CheckParseList(
                "L001",
                MakeArray(new AddressConstant("L001")),
                TokenType.EndOfToken,
                "アドレス定数");
            CheckParseList(
                "12345,#ABCD,'文字定数',L001", 
                MakeArray(
                    new DecimalConstant(12345),
                    new HexaDecimalConstant(0xABCD),
                    new StringConstant("文字定数"),
                    new AddressConstant("L001")),
                TokenType.EndOfToken,
                "定数の並び");

            CheckParseList(
                String.Empty, null, DontCare,
                "空文字列でオペランドなし => エラー, 1 つ以上の定数が必要");
            CheckParseList(
                "123,", null, DontCare,
                "コンマに続く定数なし => エラー, コンマがあれば続いて定数が必要");
            CheckParseList(
                "'abc'123",
                MakeArray(new StringConstant("abc")),
                TokenType.DecimalConstant,
                "区切りのコンマがない => コンマがなければ、解釈はそこまで");
        }

        private static void CheckParseList(
            String str, Constant[] expectedConstants, TokenType expectedLeftTokenType, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            OperandLexer lexer = new OperandLexer(buffer);
            lexer.MoveNext();
            try
            {
                Constant[] actualConstants = Constant.ParseList(lexer);

                Assert.IsNotNull(expectedConstants, message);
                TestUtils.CheckArray(expectedConstants, actualConstants, Check, message);

                TokenType actualLeftTokenType = lexer.CurrentToken.Type;
                Assert.AreEqual(expectedLeftTokenType, actualLeftTokenType, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedConstants, message);
            }
        }

        internal static Constant[] MakeArray(params Constant[] args)
        {
            return TestUtils.MakeArray<Constant>(args);
        }

        internal static T CheckRead<T>(
            Func<ReadBuffer, T> readFunc, String str, Boolean success, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                T value = readFunc(buffer);
                Assert.IsTrue(success, message);
                return value;
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
                return default(T);
            }
        }

        internal static void Check(Constant expected, Constant actual, String message)
        {
            TestUtils.CheckType(expected, actual, message);

            Type expectedType = expected.GetType();
            if (expectedType == typeof(DecimalConstant))
            {
                DecimalConstantTest.Check(
                    (DecimalConstant)expected, (DecimalConstant)actual, message);
            }
            else if (expectedType == typeof(HexaDecimalConstant))
            {
                HexaDecimalConstantTest.Check(
                    (HexaDecimalConstant)expected, (HexaDecimalConstant)actual, message);
            }
            else if (expectedType == typeof(StringConstant))
            {
                StringConstantTest.Check(
                    (StringConstant)expected, (StringConstant)actual, message);
            }
            else if (expectedType == typeof(AddressConstant))
            {
                AddressConstantTest.Check(
                    (AddressConstant)expected, (AddressConstant)actual, message);
            }
            else
            {
                Assert.Fail("未知の Constant の派生クラスです。");
            }
        }
    }
}
