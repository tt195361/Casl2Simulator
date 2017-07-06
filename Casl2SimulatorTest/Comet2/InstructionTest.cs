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
        /// LoadEaContents 命令実行の単体テストです。
        /// </summary>
        [TestMethod]
        public void ExecuteLoadEaContents()
        {
            const UInt16 R = 3;
            const UInt16 X = 4;
            const UInt16 Adr = 1234;
            const UInt16 Offset = 2345;
            const UInt16 EffectiveAddress = Adr + Offset;
            const UInt16 EaContents = 0xa5a5;

            // 命令語の次のアドレスに adr, 実効アドレスの内容、GR4 にオフセットの値を書き込みます。
            m_memory.Write(NextAddress, Adr);
            m_memory.Write(EffectiveAddress, EaContents);
            m_registerSet.GR[4].SetValue(Offset);

            // テスト対象の命令を実行します。
            ExecuteInstruction(Instruction.LoadEaContents, R, X);

            // 実行結果をチェックします。
            UInt16 expected = EaContents;
            UInt16 actual = m_registerSet.GR[3].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, "GR3 に有効アドレスの内容が設定される");
        }

        private void ExecuteInstruction(Instruction instruction, UInt16 rR1Field, UInt16 xR2Field)
        {
            m_registerSet.PR.SetValue(NextAddress);
            instruction.Execute(rR1Field, xR2Field, m_registerSet, m_memory);
        }

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(Instruction.LoadEaContents, "LD r,adr,x", "LoadEaContents");
        }

        private void CheckToString(Instruction instruction, String expected, String message)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
