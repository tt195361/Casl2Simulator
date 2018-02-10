using System;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// ExportLabel クラスの単体テストです。
    /// </summary>
    internal class ExportLabelTest
    {
        internal static void Check(ExportLabel expected, ExportLabel actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
            MemoryOffsetTest.Check(expected.CodeOffset, actual.CodeOffset, "CodeOffset: " + message);
        }
    }
}
