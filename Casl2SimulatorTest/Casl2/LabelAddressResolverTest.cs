using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;
using Tt195361.Casl2SimulatorTest.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="LabelAddressResolver"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class LabelAddressResolverTest
    {
        #region Instance Members
        private LabelAddressResolver m_labelAddrResolver;
        private LabelTable m_labelTable;
        private EntryPointTable m_entryPointTable;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_labelAddrResolver = Make();
            m_labelTable = m_labelAddrResolver.LabelTable;
            m_entryPointTable = m_labelAddrResolver.EntryPointTable;
        }

        /// <summary>
        /// <see cref="LabelAddressResolver.ResolveAddressFor"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ResolveAddressFor()
        {
            Label definedLabel = new Label("DEF");
            MemoryAddress definedLabelAddress = new MemoryAddress(0x1234);
            LabelDefinition labelDef = 
                LabelDefinition.MakeForUnitTest(definedLabel, MemoryOffset.Zero, definedLabelAddress);
            m_labelTable.RegisterForUnitTest(labelDef);
            CheckResolveAddressFor(
                definedLabel, true, definedLabelAddress,
                "プログラム内で定義されたラベル => 成功、そのラベルのアドレスが返される");

            EntryPoint entryPoint = EntryPointTest.Make("EXESTRT", "ENTRY", 0xfedc);
            m_entryPointTable.Register(entryPoint);
            CheckResolveAddressFor(
                entryPoint.EntryLabel, true, entryPoint.ExecStartAddress,
                "プログラムの入口名 => 成功、そのプログラムの実行開始アドレスが返される");

            Label undefinedLabel = new Label("UNDEF");
            MemoryAddress DontCare = MemoryAddress.Zero;
            CheckResolveAddressFor(
                undefinedLabel, false, DontCare,
                "未定義のラベル => 例外");
        }

        private void CheckResolveAddressFor(
            Label label, Boolean success, MemoryAddress expected, String message)
        {
            try
            {
                MemoryAddress actual = m_labelAddrResolver.ResolveAddressFor(label);
                Assert.IsTrue(success, message);
                MemoryAddressTest.Check(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        internal static LabelAddressResolver Make()
        {
            LabelTable labelTable = new LabelTable();
            EntryPointTable entryPointTable = new EntryPointTable();
            return new LabelAddressResolver(labelTable, entryPointTable);
        }
    }
}
