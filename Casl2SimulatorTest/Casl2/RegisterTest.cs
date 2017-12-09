using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Register クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RegisterTest
    {
        /// <summary>
        /// Name プロパティのテストです。
        /// </summary>
        [TestMethod]
        public void Name()
        {
            CheckName(Register.GR0, "GR0", "GR0 => 'GR0'");
            CheckName(Register.GR1, "GR1", "GR1 => 'GR1'");
            CheckName(Register.GR2, "GR2", "GR2 => 'GR2'");
            CheckName(Register.GR3, "GR3", "GR3 => 'GR3'");
            CheckName(Register.GR4, "GR4", "GR4 => 'GR4'");
            CheckName(Register.GR5, "GR5", "GR5 => 'GR5'");
            CheckName(Register.GR6, "GR6", "GR6 => 'GR6'");
            CheckName(Register.GR7, "GR7", "GR7 => 'GR7'");
        }

        private void CheckName(Register register, String expected, String message)
        {
            String actual = register.Name;
            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>
        /// Number プロパティのテストです。
        /// </summary>
        [TestMethod]
        public void Number()
        {
            CheckNumber(Register.GR0, 0, "GR0 => 0");
            CheckNumber(Register.GR1, 1, "GR1 => 1");
            CheckNumber(Register.GR2, 2, "GR2 => 2");
            CheckNumber(Register.GR3, 3, "GR3 => 3");
            CheckNumber(Register.GR4, 4, "GR4 => 4");
            CheckNumber(Register.GR5, 5, "GR5 => 5");
            CheckNumber(Register.GR6, 6, "GR6 => 6");
            CheckNumber(Register.GR7, 7, "GR7 => 7");
        }

        private void CheckNumber(Register register, Int32 expected, String message)
        {
            Int32 actual = register.Number;
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(Register expected, Register actual, String message)
        {
            Assert.AreSame(expected, actual, message);
        }
    }
}
