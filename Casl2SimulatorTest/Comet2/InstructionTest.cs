using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Instruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionTest
    {
        /// <summary>
        /// Decode メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Decode()
        {
            CheckDecode(0x1000, Instruction.Load, "0x1000 => Load");
            CheckDecode(0xe000, null, "0xe000 => 未定義");
        }

        private void CheckDecode(UInt16 value, Instruction expected, String message)
        {
            try
            {
                Word word = new Word(value);
                Instruction actual = Instruction.Decode(word);
                Assert.IsNotNull(expected, message);
                Assert.AreSame(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(Instruction.Load, "LD", "ロード命令 => LD");
        }

        private void CheckToString(Instruction instruction, String expected, String message)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual, message);
        }
    }
}
