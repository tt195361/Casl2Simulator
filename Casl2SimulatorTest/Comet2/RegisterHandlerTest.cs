using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// <see cref="RegisterHandler"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RegisterHandlerTest
    {
        #region Instance Fields
        private RegisterSet m_registerSet;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
        }

        /// <summary>
        /// Register のテストです。
        /// </summary>
        [TestMethod]
        public void Register()
        {
            RegisterHandler handler = RegisterHandler.Register;
            const Register DontCare = null;

            CheckRegister(handler, 0, true, m_registerSet.GR[0], "r/r1=0 => GR0");
            CheckRegister(handler, 7, true, m_registerSet.GR[7], "r/r1=7 => GR7");
            CheckRegister(handler, 8, false, DontCare, "r/r1=8 => エラー");
        }

        /// <summary>
        /// NoRegister のテストです。
        /// </summary>
        [TestMethod]
        public void NoRegister()
        {
            RegisterHandler handler = RegisterHandler.NoRegister;

            CheckRegister(handler, 0, true, null, "r/r1 に関係なく null を返す");
            CheckRegister(handler, 8, true, null, "r/r1 が 7 より大きくても問題ない");
        }

        private void CheckRegister(
            RegisterHandler handler, UInt16 rR1Field, Boolean success, Register expected, String message)
        {
            try
            {
                Register actual = handler.GetRegister(rR1Field, m_registerSet);
                Assert.IsTrue(success, message);
                Assert.AreSame(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
