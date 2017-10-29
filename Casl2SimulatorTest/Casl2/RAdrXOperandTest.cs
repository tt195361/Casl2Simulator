using System;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// RAdrXOperand クラスの単体テストです。
    /// </summary>
    internal class RAdrXOperandTest
    {
        internal static void Check(RAdrXOperand expected, RAdrXOperand actual, String message)
        {
            RegisterOperandTest.Check(expected.R, actual.R, "R: " + message);
            AdrXOperandTest.Check(expected.AdrX, actual.AdrX, "AdrX: " + message);
        }
    }
}
