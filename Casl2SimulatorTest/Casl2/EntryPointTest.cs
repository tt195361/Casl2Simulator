using System;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="EntryPoint"/> クラスの単体テストです。
    /// </summary>
    internal class EntryPointTest
    {
        internal static void Check(EntryPoint expected, EntryPoint actual, String message)
        {
            LabelTest.Check(expected.ExecStartLabel, actual.ExecStartLabel, "ExecStartLabel: " + message);
            LabelTest.Check(expected.ExportLabel, actual.ExportLabel, "ExportLabel: " + message);
        }
    }
}
