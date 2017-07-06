using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Register クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RegisterTest
    {
        #region Fields
        private Register m_register;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_register = Register.MakeGR(0);
        }

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(0x0000, "GR0: 0", "0x0000 => '0'");
            CheckToString(0xa5a5, "GR0: 42405", "0xa5a5 => '42405'");
            CheckToString(0xffff, "GR0: 65535", "0xffff => '65535'");
        }

        private void CheckToString(UInt16 value, String expected, String message)
        {
            m_register.Value = new Word(value);
            String actual = m_register.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
