using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LabelDefinition"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelDefinitionTest
    {
        /// <summary>
        /// <see cref="LabelDefinition.AssignAbsAddress"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void AssignAbsAddress()
        {
            const UInt16 DontCare = 0;

            CheckAssignAbsAddress(
                1, 2, true, 3,
                "割り当てられる絶対アドレスは、先頭アドレスにオフセットを足した値");
            CheckAssignAbsAddress(
                1, 0xfffe, true, 0xffff,
                "割り当てられる絶対アドレスがちょうど最大値 => OK");
            CheckAssignAbsAddress(
                1, 0xffff, false, DontCare,
                "割り当てられる絶対アドレスが最大値より大きい => 例外");
        }

        private void CheckAssignAbsAddress(
            UInt16 relOffsetValue, UInt16 baseAddressValue,
            Boolean success, UInt16 expectedAbsAddressValue, String message)
        {
            Label label = new Label("LBL001");
            LabelDefinition target = new LabelDefinition(label);

            MemoryOffset relOffset = new MemoryOffset(relOffsetValue);
            target.SetRelOffset(relOffset);

            MemoryAddress baseAddress = new MemoryAddress(baseAddressValue);
            try
            {
                target.AssignAbsAddress(baseAddress);
                Assert.IsTrue(success, message);

                MemoryAddress expectedAbsAddress = new MemoryAddress(expectedAbsAddressValue);
                MemoryAddress actualAbsAddress = target.AbsAddress;
                MemoryAddressTest.Check(expectedAbsAddress, actualAbsAddress, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(LabelDefinition expected, LabelDefinition actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
            MemoryOffsetTest.Check(expected.RelOffset, actual.RelOffset, "RelOffset: " + message);
            MemoryAddressTest.Check(expected.AbsAddress, actual.AbsAddress, "AbsAddress: " + message);
        }

        internal static LabelDefinition Make(String name, UInt16 relOffsetValue, UInt16 absAddressValue)
        {
            Label label = new Label(name);
            MemoryOffset relOffset = new MemoryOffset(relOffsetValue);
            MemoryAddress absAddress = new MemoryAddress(absAddressValue);
            return LabelDefinition.MakeForUnitTest(label, relOffset, absAddress);
        }
    }
}
