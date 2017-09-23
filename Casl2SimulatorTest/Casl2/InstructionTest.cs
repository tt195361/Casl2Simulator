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
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            CheckParse("START", typeof(AsmStartInstruction), "アセンブラ命令 START");
            CheckParse("DC", typeof(AsmDcInstruction), "アセンブラ命令 DC");

            CheckParse("IN", typeof(MacroInInstruction), "マクロ命令 IN");
            CheckParse("RPUSH", typeof(MacroRpushInstruction), "マクロ命令 RPUSH");

            CheckParse(String.Empty, null, "空文字列 => 例外");
            CheckParse("UNDEF", null, "未定義命令 => 例外");
        }

        private void CheckParse(String str, Type expectedType, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                Instruction instruction = Instruction.Parse(buffer);
                Assert.IsNotNull(expectedType, message);
                Assert.IsInstanceOfType(instruction, expectedType, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expectedType, message);
            }
        }

        internal static void CheckParseOperand(Instruction target, String str, Boolean success, String message)
        {
            ReadBuffer buffer = new ReadBuffer(str);
            try
            {
                target.ParseOperand(buffer);
                Assert.IsTrue(success, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsFalse(success, message);
            }
        }
    }
}
