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
        #region Fields
        private LabelManager m_lblManager;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_lblManager = new LabelManager();
        }

        /// <summary>
        /// GetAddress メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetAddress()
        {
            const UInt16 RegisteredOffset = 0x1234;
            const UInt16 NotRegisteredOffset = 0x0000;

            AddressConstant registered = new AddressConstant("REGED");
            AddressConstant notRegistered = new AddressConstant("NOTREGED");
            m_lblManager.Register(registered.Label, RegisteredOffset);

            CheckGetAddress(
                registered, RegisteredOffset,
                "登録されたラベルのアドレス定数 => アドレスはそのラベルのオフセット");
            CheckGetAddress(
                notRegistered, NotRegisteredOffset,
                "登録されていないラベルのアドレス定数 => アドレスは 0");
        }

        private void CheckGetAddress(IAdrValue target, UInt16 expected, String message)
        {
            UInt16 actual = target.GetAddress(m_lblManager);
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(AddressConstant expected, AddressConstant actual, String message)
        {
            LabelTest.Check(expected.Label, actual.Label, "Label: " + message);
        }
    }
}
