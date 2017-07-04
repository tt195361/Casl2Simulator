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
            m_register = new Register();
        }

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(0x0000, "0x0000", "0x0000 => '0x0000'");
            CheckToString(0xa5a5, "0xa5a5", "0xa5a5 => '0xa5a5'");
            CheckToString(0xffff, "0xffff", "0xffff => '0xffff'");
        }

        private void CheckToString(UInt16 value, String expected, String message)
        {
            m_register.Value = new Word(value);
            String actual = m_register.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
