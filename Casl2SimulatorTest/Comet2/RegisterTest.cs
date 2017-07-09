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
        /// Increment メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Increment()
        {
            CheckIncrement(0x7fff, 0x8000, "Increment を呼ぶと値が 1 増える");
            CheckIncrement(0xffff, 0x0000, "値が 0xffff のときは 0x0000 になる。");
        }

        private void CheckIncrement(UInt16 regValue, UInt16 expected, String message)
        {
            m_register.SetValue(regValue);
            m_register.Increment();
            UInt16 actual = m_register.Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
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
