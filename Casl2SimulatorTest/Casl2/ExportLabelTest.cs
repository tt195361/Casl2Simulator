using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// ExportLabel クラスの単体テストです。
    /// </summary>
    internal class ExportLabelTest
    {
        internal static void Check(
            ExportLabel actual, Label expectedLabel, UInt16 expectedCodeOffset, String message)
        {
            LabelTest.Check(expectedLabel, actual.Label, "Label: " + message);
            Assert.AreEqual(expectedCodeOffset, actual.CodeOffset, "CodeOffset: " + message);
        }
    }
}
