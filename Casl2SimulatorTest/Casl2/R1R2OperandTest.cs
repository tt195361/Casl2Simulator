using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// R1R2Operand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class R1R2OperandTest
    {
        #region Fields
        private const UInt16 Opcode = 0x5A;
        #endregion

        /// <summary>
        /// Parse メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void Parse()
        {
            const R1R2Operand DontCare = null;

            CheckParse(
                "GR3,GR5", true,
                R1R2Operand.MakeForUnitTest(Opcode, RegisterOperandTest.GR3, RegisterOperandTest.GR5),
                "r1,r2 => OK");

            CheckParse(
                "GRZ,GR1", false,
                DontCare,
                "最初がレジスタ名でない => 例外");
            CheckParse(
                "GR3@GR1", false,
                DontCare,
                "レジスタ名の後が ',' でない => 例外");
            CheckParse(
                "GR3,1111", false,
                DontCare,
                "レジスタ名と ',' の後がレジスタ名でない => 例外");
        }

        private void CheckParse(String str, Boolean success, R1R2Operand expected, String message)
        {
            R1R2Operand actual = MachineInstructionOperandTest.CheckParse(
                (lexer) => R1R2Operand.Parse(lexer, Opcode), str, success, message);
            if (success)
            {
                MachineInstructionOperandTest.Check(expected, actual, Check, message);
            }
        }

        /// <summary>
        /// ToString メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            R1R2Operand target = R1R2Operand.MakeForUnitTest(RegisterOperandTest.GR2, RegisterOperandTest.GR7);
            String actual = target.ToString();
            Assert.AreEqual("GR2,GR7", actual, "GR2,GR7 => 'GR2,GR7'");
        }

        internal static void Check(R1R2Operand expected, R1R2Operand actual, String message)
        {
            RegisterOperandTest.Check(expected.R1, actual.R1, "R1: " + message);
            RegisterOperandTest.Check(expected.R2, actual.R2, "R2: " + message);
        }
    }
}
