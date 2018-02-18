using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        internal static RegisterOperand GR0 = RegisterOperand.MakeForUnitTest(Register.GR0);
        internal static RegisterOperand GR1 = RegisterOperand.MakeForUnitTest(Register.GR1);
        internal static RegisterOperand GR2 = RegisterOperand.MakeForUnitTest(Register.GR2);
        internal static RegisterOperand GR3 = RegisterOperand.MakeForUnitTest(Register.GR3);
        internal static RegisterOperand GR4 = RegisterOperand.MakeForUnitTest(Register.GR4);
        internal static RegisterOperand GR5 = RegisterOperand.MakeForUnitTest(Register.GR5);
        internal static RegisterOperand GR6 = RegisterOperand.MakeForUnitTest(Register.GR6);
        internal static RegisterOperand GR7 = RegisterOperand.MakeForUnitTest(Register.GR7);

        private const UInt16 Opcode = 0xFE;
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const RegisterOperand DontCare = null;

            CheckParse(
                "GR7", true, RegisterOperand.MakeForUnitTest(Opcode, Register.GR7),
                "r => OK");
            CheckParse(
                "GRN", false, DontCare,
                "レジスタ名でない => 例外");
        }

        private void CheckParse(String str, Boolean success, RegisterOperand expected, String message)
        {
            RegisterOperand actual = OperandTest.CheckParse(
                (lexer) => RegisterOperand.Parse(lexer, Opcode), str, success, message);
            if (success)
            {
                MachineInstructionOperandTest.Check(expected, actual, Check, message);
            }
        }

        internal static void Check(RegisterOperand expected, RegisterOperand actual, String message)
        {
            RegisterTest.Check(expected.Register, actual.Register, message);
        }
    }
}
