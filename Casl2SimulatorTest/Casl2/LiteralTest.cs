using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Literal クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LiteralTest
    {
        #region Fields
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
                "LTRL0001\tDC\t12345",
                "10 進定数 => 10 進定数の DC 命令が生成される。");
            CheckGenerateLiteralDc(
                Literal.MakeForUnitTest(new HexaDecimalConstant(0xFEDC)),
                "LTRL0001\tDC\t#FEDC",
                "16 進定数 => 16 進定数の DC 命令が生成される。");
            CheckGenerateLiteralDc(
                Literal.MakeForUnitTest(new StringConstant("!@#$%")),
                "LTRL0001\tDC\t'!@#$%'",
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
            MemoryOffset LabelOffset = new MemoryOffset(0x1357);

            LabelManager lblManager = new LabelManager();
            m_target.GenerateLiteralDc(lblManager);
            lblManager.SetOffset(m_target.Label, LabelOffset);

            Word[] expectedWords = WordTest.MakeArray(LabelOffset.Value);
            ICodeGeneratorTest.CheckGenerateCode(
                m_target, lblManager, expectedWords, "生成したラベルのオフセットがコードになる");
        }

        internal static void Check(Literal expected, Literal actual, String message)
        {
            ConstantTest.Check(expected.Constant, actual.Constant, message);
        }
    }
}
