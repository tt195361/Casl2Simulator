using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="Literal"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LiteralTest
    {
        #region Instance Fields
        private Literal m_target;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_target = Literal.MakeForUnitTest(new DecimalConstant(12345));
        }

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const Constant DontCare = null;

            CheckParse("12345", true, new DecimalConstant(12345), "10 進定数 => OK");
            CheckParse("#ABCD", true, new HexaDecimalConstant(0xABCD), "16 進定数 => OK");
            CheckParse("'#$!%'", true, new StringConstant("#$!%"), "文字定数 => OK");
            CheckParse(LabelTest.ValidLabelName, false, DontCare, "それ以外 => リテラルに使えない");
        }

        private void CheckParse(String str, Boolean success, Constant expected, String message)
        {
            Literal literal = OperandTest.CheckParse(Literal.Parse, str, success, message);
            if (success)
            {
                Constant actual = literal.Constant;
                ConstantTest.Check(expected, actual, message);
            }
        }

        /// <summary>
        /// GetCodeWordCount メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetCodeWordCount()
        {
            ICodeGeneratorTest.CheckGetCodeWordCount(m_target, 1, "Literal => 1 語生成する");
        }

        /// <summary>
        /// GenerateLiteralDc メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateLiteralDc()
        {
            CheckGenerateLiteralDc(
                Literal.MakeForUnitTest(new DecimalConstant(12345)),
                LineTest.MakeGeneratedLine("LTRL0001", "DC", "12345"),
                "10 進定数 => 10 進定数の DC 命令が生成される。");
            CheckGenerateLiteralDc(
                Literal.MakeForUnitTest(new HexaDecimalConstant(0xFEDC)),
                LineTest.MakeGeneratedLine("LTRL0001", "DC", "#FEDC"),
                "16 進定数 => 16 進定数の DC 命令が生成される。");
            CheckGenerateLiteralDc(
                Literal.MakeForUnitTest(new StringConstant("!@#$%")),
                LineTest.MakeGeneratedLine("LTRL0001", "DC", "'!@#$%'"),
                "文字定数 => 文字定数の DC 命令が生成される。");
        }

        private void CheckGenerateLiteralDc(IAdrCodeGenerator literal, String expected, String message)
        {
            ICodeGeneratorTest.CheckGenerateLiteralDc(literal, expected, message);
        }

        /// <summary>
        /// GenerateCode メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GenerateCode()
        {
            LabelTable lblTable = new LabelTable();
            m_target.GenerateLiteralDc(lblTable);

            Word[] expectedWords = TestUtils.MakeArray(Word.Zero);
            ICodeGeneratorTest.CheckGenerateCode(
                m_target, expectedWords,
                "オブジェクトコードにラベルのアドレスが入る場所を確保する値 0 の語が追加される");

            // RelocatableModule に LabelReference が追加されることは、RelocatableModule のテストで確認する。
        }

        internal static void Check(Literal expected, Literal actual, String message)
        {
            ConstantTest.Check(expected.Constant, actual.Constant, message);
        }
    }
}
