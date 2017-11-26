using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Casl2;
using Tt195361.Casl2Simulator.Common;

namespace Tt195361.Casl2SimulatorTest.Casl2
{
    /// <summary>
    /// RAdrXOperand クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class RAdrXOperandTest
    {
        /// <summary>
        /// MakeSecondWord メソッドのテストです。
        /// </summary>
        [TestMethod]
        public void MakeSecondWord()
        {
            const UInt16 Adr = 0xABCD;
            RegisterOperand DontCareRegister = RegisterOperandTest.GR1;
            RAdrXOperand target = new RAdrXOperand(
                DontCareRegister, 
                AdrXOperand.MakeForUnitTest(new HexaDecimalConstant(Adr), DontCareRegister));

            MachineInstructionOperandTest.CheckMakeSecondWord(
                target, new Word(Adr), "第 2 語は adr の値になる");
        }

        internal static void Check(RAdrXOperand expected, RAdrXOperand actual, String message)
        {
            RegisterOperandTest.Check(expected.R, actual.R, "R: " + message);
            AdrXOperandTest.Check(expected.AdrX, actual.AdrX, "AdrX: " + message);
        }
    }
}
