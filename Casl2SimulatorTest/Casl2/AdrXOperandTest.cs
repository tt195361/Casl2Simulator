using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AdrXOperand"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AdrXOperandTest
    {
        #region Fields
        private AdrXOperand m_adrOnly;
        private AdrXOperand m_adrWithX;

        private const UInt16 Opcode = 0x46;
        private const UInt16 AdrOnlyValue = 1357;
        private const UInt16 AdrWithXValue = 2468;
        private readonly RegisterOperand X = RegisterOperandTest.GR7;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_adrOnly = AdrXOperand.MakeForUnitTest(new DecimalConstant(AdrOnlyValue), null);
            m_adrWithX = AdrXOperand.MakeForUnitTest(new DecimalConstant(AdrWithXValue), X);
        }

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            AdrXOperand DontCare = null;

            // adr のみの場合。
            CheckParse(
                "12345", true,
                AdrXOperand.MakeForUnitTest(Opcode, new DecimalConstant(12345), null),
                "10 進定数");
            CheckParse(
                "#ABCD", true,
                AdrXOperand.MakeForUnitTest(Opcode, new HexaDecimalConstant(0xABCD), null),
                "16 進定数");
            CheckParse(
                LabelTest.ValidLabelName, true,
                AdrXOperand.MakeForUnitTest(Opcode, new AddressConstant(LabelTest.ValidLabelName), null),
                "アドレス定数");
            CheckParse(
                "=12345", true,
                AdrXOperand.MakeForUnitTest(Opcode, Literal.MakeForUnitTest(new DecimalConstant(12345)), null),
                "リテラル");
            CheckParse(
                "'文字定数'", false,
                DontCare,
                "上記以外 => 例外");

            // adr に続く字句要素がある場合
            CheckParse(
                "23456=", true,
                AdrXOperand.MakeForUnitTest(Opcode, new DecimalConstant(23456), null),
                "続きが ',' 以外なら、そこで解釈終了し x なし");
            CheckParse(
                "23456,", false,
                DontCare, "続きが ',' なら、x が必要 => 例外");
            CheckParse(
                "#BCDE,GR1", true,
                AdrXOperand.MakeForUnitTest(Opcode, new HexaDecimalConstant(0xBCDE), RegisterOperandTest.GR1),
                "続きが ',' で x を指定");
            CheckParse(
                "#BCDE,GR0", false,
                DontCare, "GR0 は指標レジスタとして使えない => 例外");
            CheckParse(
                "#CDEF,GR2,", true,
                AdrXOperand.MakeForUnitTest(Opcode, new HexaDecimalConstant(0xCDEF), RegisterOperandTest.GR2),
                "x の後ろがあってもここでは OK");
        }

        private void CheckParse(String str, Boolean success, AdrXOperand expected, String message)
        {
            AdrXOperand actual = OperandTest.CheckParse(
                (lexer) => AdrXOperand.Parse(lexer, Opcode), str, success, message);
            if (success)
            {
                MachineInstructionOperandTest.Check(expected, actual, Check, message);
            }
        }

        /// <summary>
        /// GetXR2 メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetXR2()
        {
            CheckGetXR2(m_adrOnly, 0, "adr のみで X なし => XR2 は 0");
            CheckGetXR2(m_adrWithX, X.Number, "X あり => XR2 は X の値");
        }

        private void CheckGetXR2(AdrXOperand target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetXR2();
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            CheckGenerateCode(
                m_adrOnly, AdrOnlyValue,
                "adr にコードを生成させる => DecimalConstant の AdrOnlyValue");
            CheckGenerateCode(
                m_adrWithX, AdrWithXValue,
                "adr にコードを生成させる => DecimalConstant の AdrWithXValue");
        }

        private void CheckGenerateCode(AdrXOperand target, UInt16 expected, String message)
        {
            Word[] expectedWords = WordTest.MakeArray(expected);
            ICodeGeneratorTest.CheckGenerateCode(target, expectedWords, message);
        }

        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            CheckToString(m_adrOnly, "1357", "adr のみ => adr の値のみ");
            CheckToString(m_adrWithX, "2468,GR7", "adr と X => adr と X の値");
        }

        private void CheckToString(AdrXOperand target, String expected, String message)
        {
            String actual = target.ToString();
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(AdrXOperand expected, AdrXOperand actual, String message)
        {
            CheckAdr(expected.Adr, actual.Adr, "Adr: " + message);
            CheckX(expected.X, actual.X, "X: " + message);
        }

        private static void CheckAdr(IAdrCodeGenerator expected, IAdrCodeGenerator actual, String message)
        {
            ICodeGeneratorTest.CheckIAdrCodeGenerator(expected, actual, message);
        }

        private static void CheckX(RegisterOperand expected, RegisterOperand actual, String message)
        {
            if (expected == null)
            {
                Assert.IsNull(actual, message);
            }
            else
            {
                Assert.IsNotNull(actual, message);
                RegisterOperandTest.Check(expected, actual, message);
            }
        }
    }
}
