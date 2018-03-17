using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ProgramRegister"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ProgramRegisterTest
    {
        #region Static Fields
        internal static ProgramRegister GR0 = ProgramRegister.GetFor("GR0");
        internal static ProgramRegister GR1 = ProgramRegister.GetFor("GR1");
        internal static ProgramRegister GR2 = ProgramRegister.GetFor("GR2");
        internal static ProgramRegister GR3 = ProgramRegister.GetFor("GR3");
        internal static ProgramRegister GR4 = ProgramRegister.GetFor("GR4");
        internal static ProgramRegister GR5 = ProgramRegister.GetFor("GR5");
        internal static ProgramRegister GR6 = ProgramRegister.GetFor("GR6");
        internal static ProgramRegister GR7 = ProgramRegister.GetFor("GR7");
        #endregion

        /// <summary>
        /// <see cref="ProgramRegister.GetFor"/> メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void GetFor()
        {
            CheckGetFor("GR0", true, "'GR0' => GR0");
            CheckGetFor("GR1", true, "'GR1' => GR1");
            CheckGetFor("GR2", true, "'GR2' => GR2");
            CheckGetFor("GR3", true, "'GR3' => GR3");
            CheckGetFor("GR4", true, "'GR4' => GR4");
            CheckGetFor("GR5", true, "'GR5' => GR5");
            CheckGetFor("GR6", true, "'GR6' => GR6");
            CheckGetFor("GR7", true, "'GR7' => GR7");

            CheckGetFor("NoSuchGR", false, "'NoSuchGR' => 例外");
        }

        private void CheckGetFor(String name, Boolean success, String message)
        {
            try
            {
                ProgramRegister gr = ProgramRegister.GetFor(name);
                Assert.IsTrue(success, message);

                String expected = name;
                String actual = gr.Name;
                Assert.AreEqual(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// Name プロパティのテストです。
        /// </summary>
        [TestMethod]
        public void Name()
        {
            CheckName(GR0, "GR0", "GR0 => 'GR0'");
            CheckName(GR1, "GR1", "GR1 => 'GR1'");
            CheckName(GR2, "GR2", "GR2 => 'GR2'");
            CheckName(GR3, "GR3", "GR3 => 'GR3'");
            CheckName(GR4, "GR4", "GR4 => 'GR4'");
            CheckName(GR5, "GR5", "GR5 => 'GR5'");
            CheckName(GR6, "GR6", "GR6 => 'GR6'");
            CheckName(GR7, "GR7", "GR7 => 'GR7'");
        }

        private void CheckName(ProgramRegister register, String expected, String message)
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
            CheckNumber(GR0, 0, "GR0 => 0");
            CheckNumber(GR1, 1, "GR1 => 1");
            CheckNumber(GR2, 2, "GR2 => 2");
            CheckNumber(GR3, 3, "GR3 => 3");
            CheckNumber(GR4, 4, "GR4 => 4");
            CheckNumber(GR5, 5, "GR5 => 5");
            CheckNumber(GR6, 6, "GR6 => 6");
            CheckNumber(GR7, 7, "GR7 => 7");
        }

        private void CheckNumber(ProgramRegister register, Int32 expected, String message)
        {
            Int32 actual = register.Number;
            Assert.AreEqual(expected, actual, message);
        }

        internal static void Check(ProgramRegister expected, ProgramRegister actual, String message)
        {
            Assert.AreSame(expected, actual, message);
        }
    }
}
