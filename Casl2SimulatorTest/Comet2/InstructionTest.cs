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
        private const UInt16 R1 = 5;
        private const UInt16 R2 = 6;
        private const UInt16 Adr = 12345;
        private const UInt16 Offset = 23456;
        private const UInt16 EffectiveAddress = Adr + Offset;

        private const UInt16 NextAddressPlusOne = NextAddress + 1;
        private const Boolean DontCareBool = false;
        private const UInt16 DontCareUInt16 = 0;
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            m_registerSet = new RegisterSet();
            m_memory = new Memory();
        }
        #endregion

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
            const UInt16 R2Value = 13579;

            CheckRegisterRegister(
                Instruction.LoadRegister, DontCareUInt16, R2Value, R2Value,
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
                Instruction.AddArithmeticEaContents, 32767, 1, 32768,
                "実効アドレスの内容がレジスタに算術加算される");
            Assert.IsTrue(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローする");
        }

        /// <summary>
        /// SubtractArithmeticEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmeticEaContents()
        {
            CheckEaContentsRegister(
                Instruction.SubtractArithmeticEaContents, 1, 2, 0xffff,
                "実効アドレスの内容がレジスタから算術減算される");
            Assert.IsFalse(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローしない");
        }

        /// <summary>
        /// AddLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.AddLogicalEaContents, 32767, 1, 32768,
                "実効アドレスの内容がレジスタに論理加算される");
            Assert.IsFalse(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローしない");
        }

        /// <summary>
        /// SubtractLogicalEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogicalEaContents()
        {
            CheckEaContentsRegister(
                Instruction.SubtractLogicalEaContents, 1, 2, 0xffff,
                "実効アドレスの内容がレジスタから論理減算される");
            Assert.IsTrue(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローする");
        }

        /// <summary>
        /// AddArithmeticRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddArithmeticRegister()
        {
            CheckRegisterRegister(
                Instruction.AddArithmeticRegister, 10000, 22768, 32768,
                "レジスタ2の内容がレジスタ1に算術加算される");
            Assert.IsTrue(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローする");
        }

        /// <summary>
        /// SubtractArithmeticRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractArithmeticRegister()
        {
            CheckRegisterRegister(
                Instruction.SubtractArithmeticRegister, 0x7ffe, 0x7fff, 0xffff,
                "レジスタ2の内容がレジスタ1から算術減算される");
            Assert.IsFalse(m_registerSet.FR.OF, "算術だと -32768..32767 なのでオーバーフローしない");
        }

        /// <summary>
        /// AddLogicalRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AddLogicalRegister()
        {
            CheckRegisterRegister(
                Instruction.AddLogicalRegister, 10000, 22768, 32768,
                "レジスタ2の内容がレジスタ1に論理加算される");
            Assert.IsFalse(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローしない");
        }

        /// <summary>
        /// SubtractLogicalRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void SubtractLogicalRegister()
        {
            CheckRegisterRegister(
                Instruction.SubtractLogicalRegister, 0x7ffe, 0x7fff, 0xffff,
                "レジスタ2の内容がレジスタ1から論理減算される");
            Assert.IsTrue(m_registerSet.FR.OF, "論理だと 0..65535 なのでオーバーフローする");
        }
        #endregion // Arithmetic/Logical Operation

        #region Logic
        /// <summary>
        /// AndEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AndEaContents()
        {
            CheckEaContentsRegister(
                Instruction.AndEaContents, 0x137f, 0x5555, 0x1155,
                "実効アドレスの内容とレジスタの論理積を求める");
        }

        /// <summary>
        /// OrEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void OrEaContents()
        {
            CheckEaContentsRegister(
                Instruction.OrEaContents, 0x137f, 0x5555, 0x577f,
                "実効アドレスの内容とレジスタの論理和を求める");
        }

        /// <summary>
        /// XorEaContents 命令のテストです。
        /// </summary>
        [TestMethod]
        public void XorEaContents()
        {
            CheckEaContentsRegister(
                Instruction.XorEaContents, 0x137f, 0x5555, 0x462a,
                "実効アドレスの内容とレジスタの排他的論理和を求める");
        }

        /// <summary>
        /// AndRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void AndRegister()
        {
            CheckRegisterRegister(
                Instruction.AndRegister, 0x8cef, 0x5555, 0x0445,
                "レジスタ2の内容がレジスタ1に論理積される");
        }

        /// <summary>
        /// OrRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void OrRegister()
        {
            CheckRegisterRegister(
                Instruction.OrRegister, 0x8cef, 0x5555, 0xddff,
                "レジスタ2の内容がレジスタ1に論理和される");
        }

        /// <summary>
        /// XorRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void XorRegister()
        {
            CheckRegisterRegister(
                Instruction.XorRegister, 0x8cef, 0x5555, 0xd9ba,
                "レジスタ2の内容がレジスタ1に排他的論理和される");
        }
        #endregion

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

        /// <summary>
        /// CompareArithmeticRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareArithmeticRegister()
        {
            CheckRegisterFlags(
                Instruction.CompareArithmeticRegister, 0x8000, 0x7fff, false, true, false,
                "指定のレジスタの内容を算術比較し FR を設定する。" +
                "-32768 (0x8000) < 32767 (0x7fff) なので、サインフラグが設定され true になる");
        }

        /// <summary>
        /// CompareLogicalRegister 命令のテストです。
        /// </summary>
        [TestMethod]
        public void CompareLogicalRegister()
        {
            CheckRegisterFlags(
                Instruction.CompareLogicalRegister, 0x7fff, 0x8000, false, true, false,
                "指定のレジスタの内容を論理比較し FR を設定する。" +
                "32767 (0x7fff) < 32768 (0x8000) なので、サインフラグが設定され true になる");
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

        #region Check
        private void CheckEaContentsRegister(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, UInt16 expected, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            UInt16 actual = m_registerSet.GR[R].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsMemory(
            Instruction instruction, UInt16 regValue, UInt16 eaContents, Int32 address,
            UInt16 expected, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            Word word = m_memory.Read(address);
            UInt16 actual = word.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void CheckEaContentsFlags(
            Instruction instruction, UInt16 regValue, UInt16 eaContents,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteEaContentsInstruction(instruction, regValue, eaContents);
            CheckFlags(expectedOverflow, expectedSign, expectedZero, message);
        }

        private void CheckRegisterFlags(
            Instruction instruction, UInt16 reg1Value, UInt16 reg2Value,
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
            ExecuteRegisterInstruction(instruction, reg1Value, reg2Value);
            CheckFlags(expectedOverflow, expectedSign, expectedZero, message);
        }

        private void CheckFlags(
            Boolean expectedOverflow, Boolean expectedSign, Boolean expectedZero, String message)
        {
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
            ExecuteEaContentsInstruction(instruction, DontCareUInt16, DontCareUInt16);

            UInt16 expected = jump ? EffectiveAddress : NextAddressPlusOne;
            UInt16 actual = m_registerSet.PR.Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void ExecuteEaContentsInstruction(Instruction instruction, UInt16 regValue, UInt16 eaContents)
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

        private void CheckRegisterRegister(
            Instruction instruction, UInt16 reg1Value, UInt16 reg2Value, UInt16 expected, String message)
        {
            ExecuteRegisterInstruction(instruction, reg1Value, reg2Value);
            UInt16 actual = m_registerSet.GR[R1].Value.GetAsUnsigned();
            Assert.AreEqual(expected, actual, message);
        }

        private void ExecuteRegisterInstruction(Instruction instruction, UInt16 reg1Value, UInt16 reg2Value)
        {
            // レジスタに値を設定し、命令を実行します。
            m_registerSet.GR[R1].SetValue(reg1Value);
            m_registerSet.GR[R2].SetValue(reg2Value);
            instruction.Execute(R1, R2, m_registerSet, m_memory);
        }
        #endregion // Check

        #region ToString
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
            CheckToString(Instruction.SubtractArithmeticEaContents, "SUBA r,adr,x");
            CheckToString(Instruction.AddLogicalEaContents, "ADDL r,adr,x");
            CheckToString(Instruction.SubtractLogicalEaContents, "SUBL r,adr,x");
            CheckToString(Instruction.AddArithmeticRegister, "ADDA r1,r2");
            CheckToString(Instruction.SubtractArithmeticRegister, "SUBA r1,r2");
            CheckToString(Instruction.AddLogicalRegister, "ADDL r1,r2");
            CheckToString(Instruction.SubtractLogicalRegister, "SUBL r1,r2");

            CheckToString(Instruction.AndEaContents, "AND r,adr,x");
            CheckToString(Instruction.OrEaContents, "OR r,adr,x");
            CheckToString(Instruction.XorEaContents, "XOR r,adr,x");
            CheckToString(Instruction.AndRegister, "AND r1,r2");
            CheckToString(Instruction.OrRegister, "OR r1,r2");
            CheckToString(Instruction.XorRegister, "XOR r1,r2");

            CheckToString(Instruction.CompareArithmeticEaContents, "CPA r,adr,x");
            CheckToString(Instruction.CompareLogicalEaContents, "CPL r,adr,x");
            CheckToString(Instruction.CompareArithmeticRegister, "CPA r1,r2");
            CheckToString(Instruction.CompareLogicalRegister, "CPL r1,r2");

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
        #endregion // ToString
    }
}
