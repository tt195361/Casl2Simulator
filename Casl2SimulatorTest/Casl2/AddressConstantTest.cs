using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AddressConstant クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AddressConstantTest
    {
        internal static void Check(AddressConstant expected, AddressConstant actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
        }
    }
}
