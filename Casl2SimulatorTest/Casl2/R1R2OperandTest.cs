using System;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// R1R2Operand クラスの単体テストです。
    /// </summary>
    internal class R1R2OperandTest
    {
        internal static void Check(R1R2Operand expected, R1R2Operand actual, String message)
        {
            RegisterOperandTest.Check(expected.R1, actual.R1, "R1: " + message);
            RegisterOperandTest.Check(expected.R2, actual.R2, "R2: " + message);
        }
    }
}
