using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="EntryPoint"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class EntryPointTest
    {
        #region Instance Fields
        private LabelTable m_lblTable;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblTable = new LabelTable();
        }

        /// <summary>
        /// <see cref="EntryPoint.ResolveExecStartAddress"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ResolveExecStartAddress()
        {
            EntryPoint registeredEntryPoint = Make("EXESTRT", "REGED");
            EntryPoint unregisteredEntryPoint = Make("NOTDEF", "UNREGED");
            MemoryAddress registeredExecStartAddress = new MemoryAddress(0x1234);
            MemoryAddress DontCareAddress = MemoryAddress.Zero;
            MemoryOffset DontCareOffset = MemoryOffset.Zero;

            LabelDefinition labelDef = LabelDefinition.MakeForUnitTest(
                registeredEntryPoint.ExecStartLabel, DontCareOffset, registeredExecStartAddress);
            m_lblTable.RegisterForUnitTest(labelDef);

            CheckResolveExecStartAddress(
                registeredEntryPoint, true, registeredExecStartAddress,
                "実行開始ラベルが LabelTable に登録されている => 実行開始アドレスが解決され設定される");
            CheckResolveExecStartAddress(
                unregisteredEntryPoint, false, DontCareAddress,
                "実行開始ラベルが LabelTable に登録されていない => 例外");
        }

        private void CheckResolveExecStartAddress(
            EntryPoint target, Boolean success, MemoryAddress expected, String message)
        {
            try
            {
                target.ResolveExecStartAddress(m_lblTable);
                Assert.IsTrue(success, message);

                MemoryAddress actual = target.ExecStartAddress;
                MemoryAddressTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static void Check(EntryPoint expected, EntryPoint actual, String message)
        {
            LabelTest.Check(expected.ExecStartLabel, actual.ExecStartLabel, "ExecStartLabel: " + message);
            LabelTest.Check(expected.EntryLabel, actual.EntryLabel, "EntryLabel: " + message);
            MemoryAddressTest.Check(
                expected.ExecStartAddress, actual.ExecStartAddress, "ExecStartAddress: " + message);
        }

        internal static EntryPoint Make(String execStartName, String entryName)
        {
            Label execStartLabel = new Label(execStartName);
            Label entryLabel = new Label(entryName);
            return new EntryPoint(execStartLabel, entryLabel);
        }

        internal static EntryPoint Make(String execStartName, String entryName, UInt16 execStartAddressValue)
        {
            EntryPoint entryPoint = Make(execStartName, entryName);
            MemoryAddress execStartAddress = new MemoryAddress(execStartAddressValue);
            entryPoint.SetExecStartAddressForUnitTest(execStartAddress);
            return entryPoint;
        }
    }
}
