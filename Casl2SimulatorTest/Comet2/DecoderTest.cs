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
            CheckDecode(0x00, CpuInstruction.NoOperation, "0x00 => NOP");

            CheckDecode(0x10, CpuInstruction.LoadEaContents, "0x10 => LD r,adr,x");
            CheckDecode(0x11, CpuInstruction.Store, "0x11 => ST r,adr,x");
            CheckDecode(0x12, CpuInstruction.LoadEffectiveAddress, "0x12 => LAD r,adr,x");
            CheckDecode(0x14, CpuInstruction.LoadRegister, "0x14 => LD r1,r2");

            CheckDecode(0x20, CpuInstruction.AddArithmeticEaContents, "0x20 => ADDA r,adr,x");
            CheckDecode(0x21, CpuInstruction.SubtractArithmeticEaContents, "0x21 => SUBA r,adr,x");
            CheckDecode(0x22, CpuInstruction.AddLogicalEaContents, "0x22 => ADDL r,adr,x");
            CheckDecode(0x23, CpuInstruction.SubtractLogicalEaContents, "0x23 => SUBL r,adr,x");
            CheckDecode(0x24, CpuInstruction.AddArithmeticRegister, "0x24 => ADDA r1,r2");
            CheckDecode(0x25, CpuInstruction.SubtractArithmeticRegister, "0x25 => SUBA r1,r2");
            CheckDecode(0x26, CpuInstruction.AddLogicalRegister, "0x26 => ADDL r1,r2");
            CheckDecode(0x27, CpuInstruction.SubtractLogicalRegister, "0x27 => SUBL r1,r2");

            CheckDecode(0x30, CpuInstruction.AndEaContents, "0x30 => AND r,adr,x");
            CheckDecode(0x31, CpuInstruction.OrEaContents, "0x31 => OR r,adr,x");
            CheckDecode(0x32, CpuInstruction.XorEaContents, "0x32 => XOR r,adr,x");
            CheckDecode(0x34, CpuInstruction.AndRegister, "0x34 => AND r1,r2");
            CheckDecode(0x35, CpuInstruction.OrRegister, "0x35 => OR r1,r2");
            CheckDecode(0x36, CpuInstruction.XorRegister, "0x36 => XOR r1,r2");

            CheckDecode(0x40, CpuInstruction.CompareArithmeticEaContents, "0x40 => CPA r,adr,x");
            CheckDecode(0x41, CpuInstruction.CompareLogicalEaContents, "0x41 => CPL r,adr,x");
            CheckDecode(0x44, CpuInstruction.CompareArithmeticRegister, "0x44 => CPA r1,r2");
            CheckDecode(0x45, CpuInstruction.CompareLogicalRegister, "0x45 => CPL r1,r2");

            CheckDecode(0x50, CpuInstruction.ShiftLeftArithmeticEaContents, "0x50 => SLA r,adr,x");
            CheckDecode(0x51, CpuInstruction.ShiftRightArithmeticEaContents, "0x51 => SRA r,adr,x");
            CheckDecode(0x52, CpuInstruction.ShiftLeftLogicalEaContents, "0x52 => SLL r,adr,x");
            CheckDecode(0x53, CpuInstruction.ShiftRightLogicalEaContents, "0x53 => SRL r,adr,x");

            CheckDecode(0x61, CpuInstruction.JumpOnMinus, "0x61 => JMI adr,x");
            CheckDecode(0x62, CpuInstruction.JumpOnNonZero, "0x62 => JNZ adr,x");
            CheckDecode(0x63, CpuInstruction.JumpOnZero, "0x63 => JZE adr,x");
            CheckDecode(0x64, CpuInstruction.UnconditionalJump, "0x64 => JUMP adr,x");
            CheckDecode(0x65, CpuInstruction.JumpOnPlus, "0x65 => JPL adr,x");
            CheckDecode(0x66, CpuInstruction.JumpOnOverflow, "0x66 => JOV adr,x");

            CheckDecode(0x70, CpuInstruction.Push, "0x70 => PUSH adr,x");
            CheckDecode(0x71, CpuInstruction.Pop, "0x71 => POP r");

            CheckDecode(0x80, CpuInstruction.CallSubroutine, "0x80 => CALL adr,x");
            CheckDecode(0x81, CpuInstruction.ReturnFromSubroutine, "0x81 => RET");

            CheckDecode(0xf0, CpuInstruction.SuperVisorCall, "0xf0 => SVC adr,x");

            CheckDecode(0xff, null, "その他 => 未定義");
        }

        private void CheckDecode(UInt16 opcode, CpuInstruction expected, String message)
        {
            try
            {
                CpuInstruction actual = Decoder.Decode(opcode);
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
