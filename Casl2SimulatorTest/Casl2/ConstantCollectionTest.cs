using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ConstantCollection"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ConstantCollectionTest
    {
        #region Instance Fields
        private ConstantCollection m_constants;

        private const UInt16 DecimalValue = 12345;
        private const UInt16 HexaDecimalValue = 0xABCD;
        private const String StringValue = "ABC";
        private const UInt16 AValue = 0x0041;
        private const UInt16 BValue = 0x0042;
        private const UInt16 CValue = 0x0043;

        private const TokenType DontCare = null;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            DecimalConstant decimalConstant = new DecimalConstant(DecimalValue);
            HexaDecimalConstant hexaDecimalConstant = new HexaDecimalConstant(HexaDecimalValue);
            AddressConstant addressConstant = new AddressConstant("LBL001");
            StringConstant stringConstant = new StringConstant(StringValue);
            m_constants = ConstantCollection.MakeForUnitTest(
                decimalConstant, hexaDecimalConstant, addressConstant, stringConstant);
        }

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

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(
                m_constants, 1 + 1 + 1 + StringValue.Length, "それぞれの Constant の語数の合計");
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            const UInt16 LabelPlaceHolder = 0x0000;

            ICodeGeneratorTest.CheckGenerateCode(
                m_constants,
                WordTest.MakeArray(
                    DecimalValue, HexaDecimalValue, LabelPlaceHolder,
                    AValue, BValue, CValue), 
                "それぞれの Constant のコードが順に生成される");
        }

        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            String actual = m_constants.ToString();
            const String Expected = "12345,#ABCD,LBL001,'ABC'";
            Assert.AreEqual(Expected, actual, "それぞれの Constant が ',' で区切られる");
        }

        internal static void Check(
            IEnumerable<Constant> expected, IEnumerable<Constant> actual, String message)
        {
            TestUtils.CheckEnumerable(expected, actual, ConstantTest.Check, message);
        }
    }
}
