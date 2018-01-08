using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// Instruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionTest
    {
        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            CheckToString(
                "END", String.Empty, "END",
                "オペランドなし => ニーモニックのみ");
            CheckToString(
                "START", String.Empty, "START",
                "オペランドが空文字列 => ニーモニックのみ");
            CheckToString(
                "START", "EXECADDR", "START EXECADDR",
                "オペランドあり => ニーモニック + 空白 + オペランド");
            CheckToString(
                "LD", "GR3,='ABCD',GR6", "LD GR3,='ABCD',GR6",
                "オペランドが複雑な機械語命令");
        }

        private void CheckToString(String mnemonic, String operandString, String expected, String message)
        {
            Instruction target = Make(mnemonic, operandString);
            String actual = target.ToString();
            Assert.AreEqual(expected, actual, message);
        }

        internal static Instruction Make(String instructionField, String operandField)
        {
            Instruction instruction = InstructionFactory.Make(instructionField);
            ReadBuffer buffer = new ReadBuffer(operandField);
            instruction.ReadOperand(buffer);
            return instruction;
        }

        internal static void CheckReadOperand(Instruction target, String text, Boolean success, String message)
        {
            ReadBuffer buffer = new ReadBuffer(text);
            try
            {
                target.ReadOperand(buffer);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
