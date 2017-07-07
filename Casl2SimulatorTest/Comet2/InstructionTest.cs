using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Instruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionTest
    {
        #region Fields
        private RegisterSet m_registerSet;
        private Memory m_memory;

        // 命令語の次のアドレス。
        private const UInt16 NextAddress = 100;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
        }

        /// <summary>
        /// LoadEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEaContents()
        {
            CheckEaContents(
                Instruction.LoadEaContents, 0, 1357, 1357,
                "実効アドレスの内容がレジスタに設定される");
        }

        /// <summary>
        /// AddArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticEaContents()
        {
            CheckEaContents(
                Instruction.AddArithmeticEaContents, 1234, 2345, 3579,
                "実効アドレスの内容がレジスタに算術加算される");
        }

        private void CheckEaContents(
            Instruction instruction, UInt16 regValue, UInt16 oprValue, UInt16 expected, String message)
        {
            const UInt16 R = 3;
            const UInt16 X = 4;
            const UInt16 Adr = 1234;
            const UInt16 Offset = 2345;
            const UInt16 EffectiveAddress = Adr + Offset;

            // 命令語の次のアドレスに adr, 実効アドレスの内容、GR4 にオフセットの値を書き込みます。
            m_memory.Write(NextAddress, Adr);
            m_memory.Write(EffectiveAddress, oprValue);
            m_registerSet.GR[X].SetValue(Offset);

            // レジスタと PR に値を設定し、命令を実行します。
            m_registerSet.GR[R].SetValue(regValue);
            m_registerSet.PR.SetValue(NextAddress);
            instruction.Execute(R, X, m_registerSet, m_memory);

            // 実行結果をチェックします。
            UInt16 actual = m_registerSet.GR[R].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }
            /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(Instruction.LoadEaContents, "LD r,adr,x", "LoadEaContents");
            CheckToString(Instruction.AddArithmeticEaContents, "ADDA r,adr,x", "AddArithmetic");
        }

        private void CheckToString(Instruction instruction, String expected, String message)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
