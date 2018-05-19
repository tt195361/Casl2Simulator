using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="EntryPointTable"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class EntryPointTableTest
    {
        #region Instance Members
        private EntryPointTable m_entryPointTable;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_entryPointTable = new EntryPointTable();
        }

        /// <summary>
        /// <see cref="EntryPointTable.Register"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Register()
        {
            EntryPoint entryPoint = EntryPointTest.Make("EXESTRT", "ENTRY");

            CheckRegister(entryPoint, true, "まだ登録されていない入口名 => OK、登録できる");
            CheckRegister(entryPoint, false, "すでに登録された入口名 => 例外");
        }

        private void CheckRegister(EntryPoint entryPoint, Boolean success, String message)
        {
            try
            {
                m_entryPointTable.Register(entryPoint);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
