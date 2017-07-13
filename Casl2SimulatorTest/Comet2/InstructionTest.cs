using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tt195361.Casl2Simulator.Comet2;

namespace Tt195361.Casl2SimulatorTest.Comet2
{
    /// <summary>
    /// Instruction クラスの単体テストです。
    /// </summary>
    [TestClass]
    public class InstructionTest
    {
        #region Fields
        private RegisterSet m_registerSet;
        private Memory m_memory;

        // 命令語の次のアドレス。
        private const UInt16 NextAddress = 100;
        private const UInt16 R = 3;
        private const UInt16 X = 4;
        private const UInt16 Adr = 12345;
        private const UInt16 Offset = 23456;
        private const UInt16 R2 = Offset;
        private const UInt16 EffectiveAddress = Adr + Offset;

        private const UInt16 NextAddressPlusOne = NextAddress + 1;
        private const Boolean DontCareBool = false;
        private const UInt16 DontCareUInt16 = 0;
        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
        }

        #region Load/Store
        /// <summary>
        /// LoadEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEaContents()
        {
            CheckEaContentsRegister(
                Instruction.LoadEaContents, DontCareUInt16, 1357, 1357,
                "実効アドレスの内容がレジスタに設定される");
        }

        /// <summary>
        /// Store 命令のテストです。
        /// </summary>
        [TestMethod]
        public void Store()
        {
            CheckEaContentsMemory(
                Instruction.Store, 345, DontCareUInt16, EffectiveAddress, 345,
                "レジスタの内容が実効アドレスに書き込まれる");
        }

        /// <summary>
        /// LoadEffectiveAddress 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadEffectiveAddress()
        {
            CheckEaContentsRegister(
                Instruction.LoadEffectiveAddress, DontCareUInt16, DontCareUInt16, EffectiveAddress,
                "実効アドレスがレジスタに設定される");
        }

        /// <summary>
        /// LoadRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void LoadRegister()
        {
            CheckEaContentsRegister(
                Instruction.LoadRegister, DontCareUInt16, DontCareUInt16, R2,
                "指定のレジスタの内容がレジスタに設定される");
        }
        #endregion // Load/Store

        #region Arithmetic/Logical Operation
        /// <summary>
        /// AddArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.AddArithmeticEaContents, 1234, 2345, 3579,
                "実効アドレスの内容がレジスタに算術加算される");
        }
        #endregion // Arithmetic/Logical Operation

        #region Comparison
        /// <summary>
        /// CompareArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmeticEaContents()
        {
            CheckEaContentsFlags(
                Instruction.CompareArithmeticEaContents, 0xffff, 0x0001, false, true, false,
                "実効アドレスの内容とレジスタを算術比較し FR を設定する。" +
                "-1 (0xffff) < 1 (0x0001) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// CompareLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogicalEaContents()
        {
            CheckEaContentsFlags(
                Instruction.CompareLogicalEaContents, 0x0001, 0xffff, false, true, false,
                "実効アドレスの内容とレジスタを論理比較し FR を設定する。" +
                "1 (0x0001) < 65535 (0xffff) なので、サインフラグが設定され true になる");
        }
        #endregion // Comparison

        #region Shift
        /// <summary>
        /// ShiftLeftArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftLeftArithmeticEaContents, 0xaaaa, 1, 0xd554,
                "レジスタの内容が実効アドレス回だけ左に算術シフトされる");
        }

        /// <summary>
        /// ShiftRightArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftRightArithmeticEaContents, 0xaaaa, 1, 0xd555,
                "レジスタの内容が実効アドレス回だけ右に算術シフトされる");
        }

        /// <summary>
        /// ShiftLeftLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftLeftLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftLeftLogicalEaContents, 0x5555, 1, 0xaaaa,
                "レジスタの内容が実効アドレス回だけ左に論理シフトされる");
        }

        /// <summary>
        /// ShiftRightLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void ShiftRightLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.ShiftRightLogicalEaContents, 0xaaaa, 1, 0x5555,
                "レジスタの内容が実効アドレス回だけ右に論理シフトされる");
        }
        #endregion // Shift

        #region Jump
        /// <summary>
        /// JumpOnMinus 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnMinus()
        {
            CheckJump(
                Instruction.JumpOnMinus, DontCareBool, true, DontCareBool,
                true, "JumpOnMinus: SF=1 => 分岐する");
            CheckJump(
                Instruction.JumpOnMinus, DontCareBool, false, DontCareBool,
                false, "JumpOnMinus: SF=0 => 分岐しない");
        }

        /// <summary>
        /// JumpOnNonZero 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnNonZero()
        {
            CheckJump(
                Instruction.JumpOnNonZero, DontCareBool, DontCareBool, false, 
                true, "JumpOnNonZero: ZF=0 => 分岐する");
            CheckJump(
                Instruction.JumpOnNonZero, DontCareBool, DontCareBool, true, 
                false, "JumpOnNonZero: ZF=1 => 分岐しない");
        }

        /// <summary>
        /// JumpOnZero 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnZero()
        {
            CheckJump(
                Instruction.JumpOnZero, DontCareBool, DontCareBool, true,
                true, "JumpOnZero: ZF=1 => 分岐する");
            CheckJump(
                Instruction.JumpOnZero, DontCareBool, DontCareBool, false,
                false, "JumpOnZero: ZF=0 => 分岐しない");
        }

        /// <summary>
        /// UnconditionalJump 命令のテストです。
        /// </summary>
        [TestMethod]
        public void UnconditionalJump()
        {
            CheckJump(
                Instruction.UnconditionalJump, DontCareBool, DontCareBool, DontCareBool,
                true, "UnconditionalJump: 無条件で分岐する");
        }

        /// <summary>
        /// JumpOnPlus 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnPlus()
        {
            CheckJump(
                Instruction.JumpOnPlus, DontCareBool, false, false,
                true, "JumpOnPlus: SF=0 かつ ZF=0 => 分岐する");
            CheckJump(
                Instruction.JumpOnPlus, DontCareBool, true, true,
                false, "JumpOnPlus: SF=1 あるいは ZF=1 => 分岐しない");
        }

        /// <summary>
        /// JumpOnOverflow 命令のテストです。
        /// </summary>
        [TestMethod]
        public void JumpOnOverflow()
        {
            CheckJump(
                Instruction.JumpOnOverflow, true, DontCareBool, DontCareBool,
                true, "JumpOnOverflow: OF=1 => 分岐する");
            CheckJump(
                Instruction.JumpOnOverflow, false, DontCareBool, DontCareBool,
                false, "JumpOnOverflow: OF=0 => 分岐しない");
        }
        #endregion

        private void CheckEaContentsRegister(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, UInt16 expected, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            UInt16 actual = m_registerSet.GR[R].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsMemory(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, Int32 address,
            UInt16 expected, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            Word word = m_memory.Read(address);
            UInt16 actual = word.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsFlags(
            Instruction instruction, UInt16 regValue, UInt16 eaContents,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteInstruction(instruction, regValue, eaContents);
            CheckFlag(expectedOverflow, m_registerSet.FR.OF, "Overflow: " + message);
            CheckFlag(expectedSign, m_registerSet.FR.SF, "Sign: " + message);
            CheckFlag(expectedZero, m_registerSet.FR.ZF, "Zero: " + message);
        }

        private void CheckFlag(Boolean expected, Boolean actual, String message)
        {
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckJump(
            Instruction instruction, Boolean overflowFlag, Boolean signFlag, Boolean zeroFlag,
            Boolean jump, String message)
        {
            m_registerSet.FR.SetFlags(overflowFlag, signFlag, zeroFlag);
            ExecuteInstruction(instruction, DontCareUInt16, DontCareUInt16);

            UInt16 expected = jump ? EffectiveAddress : NextAddressPlusOne;
            UInt16 actual = m_registerSet.PR.Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void ExecuteInstruction(Instruction instruction, UInt16 regValue, UInt16 eaContents)
        {
            // 命令語の次のアドレスに adr, 実効アドレスの内容、GRx にオフセットの値を書き込みます。
            m_memory.Write(NextAddress, Adr);
            m_memory.Write(EffectiveAddress, eaContents);
            m_registerSet.GR[X].SetValue(Offset);

            // レジスタと PR に値を設定し、命令を実行します。
            m_registerSet.GR[R].SetValue(regValue);
            m_registerSet.PR.SetValue(NextAddress);
            instruction.Execute(R, X, m_registerSet, m_memory);
        }

        /// <summary>
        /// ToString メソッドの単体テストです。
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            CheckToString(Instruction.LoadEaContents, "LD r,adr,x");
            CheckToString(Instruction.Store, "ST r,adr,x");
            CheckToString(Instruction.LoadEffectiveAddress, "LAD r,adr,x");
            CheckToString(Instruction.LoadRegister, "LD r1,r2");

            CheckToString(Instruction.AddArithmeticEaContents, "ADDA r,adr,x");

            CheckToString(Instruction.CompareArithmeticEaContents, "CPA r,adr,x");
            CheckToString(Instruction.CompareLogicalEaContents, "CPL r,adr,x");

            CheckToString(Instruction.ShiftLeftArithmeticEaContents, "SLA r,adr,x");
            CheckToString(Instruction.ShiftRightArithmeticEaContents, "SRA r,adr,x");
            CheckToString(Instruction.ShiftLeftLogicalEaContents, "SLL r,adr,x");
            CheckToString(Instruction.ShiftRightLogicalEaContents, "SRL r,adr,x");

            CheckToString(Instruction.JumpOnMinus, "JMI adr,x");
            CheckToString(Instruction.JumpOnNonZero, "JNZ adr,x");
            CheckToString(Instruction.JumpOnZero, "JZE adr,x");
            CheckToString(Instruction.UnconditionalJump, "JUMP adr,x");
            CheckToString(Instruction.JumpOnPlus, "JPL adr,x");
            CheckToString(Instruction.JumpOnOverflow, "JOV adr,x");
        }

        private void CheckToString(Instruction instruction, String expected)
        {
            String actual = instruction.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
