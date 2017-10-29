using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// RegisterOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RegisterOperandTest
    {
        #region Static Fields
        internal static RegisterOperand GR0 = RegisterOperand.GetFor(Casl2Defs.GR0);
        internal static RegisterOperand GR1 = RegisterOperand.GetFor(Casl2Defs.GR1);
        internal static RegisterOperand GR2 = RegisterOperand.GetFor(Casl2Defs.GR2);
        internal static RegisterOperand GR3 = RegisterOperand.GetFor(Casl2Defs.GR3);
        #endregion

        /// <summary>
        /// GetFor メソッドで取得したオブジェクトの Name, Number, CanIndex プロパティを確認します。
        /// </summary>
        [TestMethod]
        public void GetFor_NameNumberCanIndex()
        {
            const Int32 DontCareNumber = -1;
            const Boolean DontCareCanIndex = false;

            CheckGetFor("GR0", true, 0, false, "GR0 => 番号=0, 指標レジスタとして使え*ない*");
            CheckGetFor("GR1", true, 1, true, "GR1 => 番号=1, 指標レジスタとして使える");
            CheckGetFor("GR2", true, 2, true, "GR2 => 番号=2, 指標レジスタとして使える");
            CheckGetFor("GR3", true, 3, true, "GR3 => 番号=3, 指標レジスタとして使える");
            CheckGetFor("GR4", true, 4, true, "GR4 => 番号=4, 指標レジスタとして使える");
            CheckGetFor("GR5", true, 5, true, "GR5 => 番号=5, 指標レジスタとして使える");
            CheckGetFor("GR6", true, 6, true, "GR6 => 番号=6, 指標レジスタとして使える");
            CheckGetFor("GR7", true, 7, true, "GR7 => 番号=7, 指標レジスタとして使える");

            CheckGetFor("GR8", false, DontCareNumber, DontCareCanIndex, "GR0..7 以外 => 未定義のレジスタ名");
        }

        private void CheckGetFor(
            String name, Boolean success, Int32 expectedNumber, Boolean expectedCanIndex, String message)
        {
            try
            {
                RegisterOperand actual = RegisterOperand.GetFor(name);
                Assert.IsTrue(success, message);
                Check(name, expectedNumber, expectedCanIndex, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }

        /// <summary>
        /// IndexRegisters の値のテストです。
        /// </summary>
        [TestMethod]
        public void IndexRegisters()
        {
            const String Expected = "GR1~GR7";
            String actual = RegisterOperand.IndexRegisters;
            Assert.AreEqual(Expected, actual, "指標レジスタとして使えるレジスタ");
        }

        internal static void Check(RegisterOperand expected, RegisterOperand actual, String message)
        {
            Assert.AreSame(expected, actual, message);
        }

        private void Check(
            String expectedName, Int32 expectedNumber, Boolean expectedCanIndex,
            RegisterOperand actual, String message)
        {
            Assert.AreEqual(expectedName, actual.Name, "Name: " + message);
            Assert.AreEqual(expectedNumber, actual.Number, "Number: " + message);
            Assert.AreEqual(expectedCanIndex, actual.CanIndex, "CanIndex: " + message);
        }
    }
}
