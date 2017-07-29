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
            CheckDecode(0x12, Instruction.LoadEffectiveAddress, "0x12 => LAD r,adr,x");
            CheckDecode(0x14, Instruction.LoadRegister, "0x14 => LD r1,r2");

            CheckDecode(0x20, Instruction.AddArithmeticEaContents, "0x20 => ADDA r,adr,x");
            CheckDecode(0x21, Instruction.SubtractArithmeticEaContents, "0x21 => SUBA r,adr,x");
            CheckDecode(0x22, Instruction.AddLogicalEaContents, "0x22 => ADDL r,adr,x");
            CheckDecode(0x23, Instruction.SubtractLogicalEaContents, "0x23 => SUBL r,adr,x");
            CheckDecode(0x24, Instruction.AddArithmeticRegister, "0x24 => ADDA r1,r2");
            CheckDecode(0x25, Instruction.SubtractArithmeticRegister, "0x25 => SUBA r1,r2");
            CheckDecode(0x26, Instruction.AddLogicalRegister, "0x26 => ADDL r1,r2");
            CheckDecode(0x27, Instruction.SubtractLogicalRegister, "0x27 => SUBL r1,r2");

            CheckDecode(0x30, Instruction.AndEaContents, "0x30 => AND r,adr,x");
            CheckDecode(0x31, Instruction.OrEaContents, "0x31 => OR r,adr,x");
            CheckDecode(0x32, Instruction.XorEaContents, "0x32 => XOR r,adr,x");
            CheckDecode(0x34, Instruction.AndRegister, "0x34 => AND r1,r2");
            CheckDecode(0x35, Instruction.OrRegister, "0x35 => OR r1,r2");
            CheckDecode(0x36, Instruction.XorRegister, "0x36 => XOR r1,r2");

            CheckDecode(0x40, Instruction.CompareArithmeticEaContents, "0x40 => CPA r,adr,x");
            CheckDecode(0x41, Instruction.CompareLogicalEaContents, "0x41 => CPL r,adr,x");

            CheckDecode(0x50, Instruction.ShiftLeftArithmeticEaContents, "0x50 => SLA r,adr,x");
            CheckDecode(0x51, Instruction.ShiftRightArithmeticEaContents, "0x51 => SRA r,adr,x");
            CheckDecode(0x52, Instruction.ShiftLeftLogicalEaContents, "0x52 => SLL r,adr,x");
            CheckDecode(0x53, Instruction.ShiftRightLogicalEaContents, "0x53 => SRL r,adr,x");

            CheckDecode(0x61, Instruction.JumpOnMinus, "0x61 => JMI adr,x");
            CheckDecode(0x62, Instruction.JumpOnNonZero, "0x62 => JNZ adr,x");
            CheckDecode(0x63, Instruction.JumpOnZero, "0x63 => JZE adr,x");
            CheckDecode(0x64, Instruction.UnconditionalJump, "0x64 => JUMP adr,x");
            CheckDecode(0x65, Instruction.JumpOnPlus, "0x65 => JPL adr,x");
            CheckDecode(0x66, Instruction.JumpOnOverflow, "0x66 => JOV adr,x");

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
