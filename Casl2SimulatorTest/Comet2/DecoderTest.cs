using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Decoder クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class DecoderTest
    {
        /// <summary>
        /// Decode メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void Decode()
        {
            CheckDecode(0x10, Instruction.LoadEaContents, "0x10 => LD r,adr,x");
            CheckDecode(0x11, Instruction.Store, "0x11 => ST r,adr,x");

            CheckDecode(0x20, Instruction.AddArithmeticEaContents, "0x20 => ADDA r,adr,x");

            CheckDecode(0x40, Instruction.CompareArithmeticEaContents, "0x40 => CPA r,adr,x");
            CheckDecode(0x41, Instruction.CompareLogicalEaContents, "0x41 => CPL r,adr,x");

            CheckDecode(0x50, Instruction.ShiftLeftArithmeticEaContents, "0x50 => SLA r,adr,x");
            CheckDecode(0x51, Instruction.ShiftRightArithmeticEaContents, "0x51 => SRA r,adr,x");
            CheckDecode(0x52, Instruction.ShiftLeftLogicalEaContents, "0x52 => SLL r,adr,x");
            CheckDecode(0x53, Instruction.ShiftRightLogicalEaContents, "0x53 => SRL r,adr,x");

            CheckDecode(0xe0, null, "0xe0 => 未定義");
        }

        private void CheckDecode(UInt16 opcode, Instruction expected, String message)
        {
            try
            {
                Instruction actual = Decoder.Decode(opcode);
                Assert.IsNotNull(expected, message);
                Assert.AreSame(expected, actual, message);
            }
            catch (Casl2SimulatorException)
            {
                Assert.IsNull(expected, message);
            }
        }
    }
}
