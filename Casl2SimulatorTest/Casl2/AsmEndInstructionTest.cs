using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// <see cref="AsmEndInstruction"/> クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class AsmEndInstructionTest
    {
        /// <summary>
        /// ReadOperand メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ReadOperand()
        {
            CheckReadOperand(
                String.Empty, true, "オペランドなし => OK");
            CheckReadOperand(
                "OPR", false, "オペランドがある => 例外");
        }

        private void CheckReadOperand(String text, Boolean success, String message)
        {
            AsmEndInstruction target = new AsmEndInstruction();
            ProgramInstructionTest.CheckReadOperand(target, text, success, message);
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
