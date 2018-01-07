using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Constant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ConstantTest
    {
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

        internal static void CheckGenerateCode(Constant target, Word[] expectedWords, String message)
        {
            LabelManager lblManager = new LabelManager();
            CheckGenerateCode(target, lblManager, expectedWords, message);
        }

        internal static void CheckGenerateCode(
            Constant target, LabelManager lblManager, Word[] expectedWords, String message)
        {
            RelocatableModule relModule = new RelocatableModule();
            target.GenerateCode(lblManager, relModule);
            RelocatableModuleTest.Check(relModule, expectedWords, message);
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
