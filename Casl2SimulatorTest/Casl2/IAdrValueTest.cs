using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    internal class IAdrValueTest
    {
        internal static void CheckGetAddress(IAdrValue target, UInt16 expected, String message)
        {
            LabelManager lblManager = new LabelManager();
            CheckGetAddress(target, lblManager, expected, message);
        }

        internal static void CheckGetAddress(
            IAdrValue target, LabelManager lblManager, UInt16 expected, String message)
        {
            UInt16 actual = target.GetAddress(lblManager);
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(IAdrValue expected, IAdrValue actual, String message)
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
                Assert.Fail("未知の IAdrValue の実装クラスです。");
            }
        }
    }
}
