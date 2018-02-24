using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="ProgramInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class ProgramInstructionTest
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
            ProgramInstruction target = Make(mnemonic, operandString);
            String actual = target.ToString();
            Assert.AreEqual(expected, actual, message);
        }

        internal static ProgramInstruction Make(String instructionField, String operandField)
        {
            ProgramInstruction instruction = ProgramInstructionFactory.Make(instructionField);
            ReadBuffer buffer = new ReadBuffer(operandField);
            instruction.ReadOperand(buffer);
            return instruction;
        }

        internal static void CheckReadOperand(
            ProgramInstruction target, String text, Boolean success, String message)
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
