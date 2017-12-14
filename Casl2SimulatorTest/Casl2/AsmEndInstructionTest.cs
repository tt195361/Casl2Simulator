using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// AsmEndInstruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmEndInstructionTest
    {
        /// <summary>
        /// ParseOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ParseOperand()
        {
            CheckParseOperand(
                String.Empty, true, "オペランドなし => OK");
            CheckParseOperand(
                "OPR", false, "オペランドがある => 例外");
        }

        private void CheckParseOperand(String text, Boolean success, String message)
        {
            AsmEndInstruction target = new AsmEndInstruction();
            InstructionTest.CheckParseOperand(target, text, success, message);
        }

        /// <summary>
        /// IsEnd メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void IsEnd()
        {
            AsmEndInstruction target = new AsmEndInstruction();
            Boolean result = target.IsEnd();
            Assert.IsTrue(result, "END 命令の IsEnd() => true");
        }
    }
}
