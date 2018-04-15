using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ICodeGenerator"/> インタフェースの単体テストで共通に使用するメソッドです。
    /// </summary>
    internal class ICodeGeneratorTest
    {
        internal static void CheckGetCodeWordCount(ICodeGenerator target, Int32 expected, String message)
        {
            Int32 actual = target.GetCodeWordCount();
            Assert.AreEqual(expected, actual, message);
        }

        internal static void CheckGenerateCode(ICodeGenerator target, Word[] expectedWords, String message)
        {
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(relModule);
            RelocatableModuleTest.CheckWords(relModule, expectedWords, message);
        }

        internal static void CheckGenerateLiteralDc(IAdrCodeGenerator target, String expected, String message)
        {
            LabelManager lblManager = new LabelManager();
            String actual = target.GenerateLiteralDc(lblManager);
            Assert.AreEqual(expected, actual, message);
        }

        internal static void CheckIAdrCodeGenerator(
            IAdrCodeGenerator expected, IAdrCodeGenerator actual, String message)
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
            else if (expectedType == typeof(AddressConstant))
            {
                AddressConstantTest.Check(
                    (AddressConstant)expected, (AddressConstant)actual, message);
            }
            else if (expectedType == typeof(Literal))
            {
                LiteralTest.Check(
                    (Literal)expected, (Literal)actual, message);
            }
            else
            {
                Assert.Fail("未知の IAddrCodeGenerator の実装クラスです。");
            }
        }
    }
}
